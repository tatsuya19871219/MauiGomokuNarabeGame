using System.Diagnostics;
using CommunityToolkit.Mvvm.Messaging;
using MauiGomokuNarabeGame.Helpers;
using MauiGomokuNarabeGame.Messages;

namespace MauiGomokuNarabeGame.Views;

public partial class GameField : ContentView
{
    #region Properties    
    public static readonly BindableProperty CoinSizeProperty
        = BindableProperty.Create(nameof(CoinSize), typeof(double), typeof(CoinPool));
    public double CoinSize
    {
        get => (double)GetValue(CoinSizeProperty);
        set => SetValue(CoinSizeProperty, value);
    }

    required public int Stacks { get; init; }
    required public int Lanes { get; init; }

    #endregion

    List<Queue<Image>> _coinQueues = new();

    public GameField()
	{
		InitializeComponent();

        WeakReferenceMessenger.Default.Send(new InitializingMessage(this));

        new ConditionalAction(
                action: () => 
                {
                    foreach(var _ in Enumerable.Range(0, Lanes)) _coinQueues.Add(new());
                    WeakReferenceMessenger.Default.Send(new InitializedMessage(this));
                },
                () => new[] {Stacks, Lanes}.All(value => value > 0),
                () => Lanes > 0
            ){ MillisecondsTimeout = 100 }.Invoke();


        StrongReferenceMessenger.Default.Register<InsertCoinRequestMessage>(this, (r, m) =>
        {
            Image coinImage = m.CoinImage;
            var targetLane = m.TargetLane;

            _coinQueues[targetLane].Enqueue(coinImage);

            var stackPosition = _coinQueues[targetLane].Count;
            
            m.Reply(InsertCoinAsync(coinImage, targetLane, stackPosition));

        });

        StrongReferenceMessenger.Default.Register<ClearFieldRequestMessage>(this, (r, m) =>
        {
            m.Reply(RemoveCoinQueuesAsync(_coinQueues));
        });
	}

    async Task<bool> InsertCoinAsync(Image coinImage, int targetLane, int stackPosition)
    {
        double insertSpeedRatio = 3.0;

        var (targetX, targetY) = GetCoinLocation(targetLane, stackPosition);

        var x0 = targetX;
        var y0 = -100.0;

        var x1 = x0;
        var y1 = targetY;

        var length = (uint) (Math.Abs(y1-y0)/insertSpeedRatio);

        coinImage.TranslationX = x0;
        coinImage.TranslationY = y0;
        
        FieldGrid.Add(coinImage);

        await coinImage.TranslateTo(x1, y1, length);

        return true;
    }

    async Task<bool> RemoveCoinQueuesAsync(List<Queue<Image>> queues)
    {        
        var tasks = queues.Select(queue => RemoveCoinsAsync(queue));

        await Task.WhenAll(tasks);

        var results = tasks.Select(q => q.Result).ToArray();

        FieldGrid.Clear();

        return results.All(r=>r);
    }

    async Task<bool> RemoveCoinsAsync(Queue<Image> queue)
    {
        var tasks = new List<Task>();

        while (queue.Count > 0)
        {
            var image = queue.Dequeue();
            
            tasks.Add( DropCoin(image) );
            
            await Task.Delay(100);
        }

        await Task.WhenAll(tasks);

        Debug.Assert(queue.Count == 0);

        return queue.Count == 0;
    }

    async Task DropCoin(Image image)
    {
        await image.TranslateTo(image.TranslationX, image.TranslationY + Height, 250);
    }

    private void ContentView_SizeChanged(object sender, EventArgs e)
    {
        if (Width <= 0 || Height <= 0) return;

        foreach(var (queue, laneIndex) in _coinQueues.Select((queue,  index) => (queue, index)))
        {
            foreach (var (coin, stackPosition) in queue.Select((coin, index) => (coin, index+1)))
            {
                var (x, y) = GetCoinLocation(laneIndex, stackPosition);

                coin.TranslationX = x;
                coin.TranslationY = y;
            }
        }
    }

    Point GetCoinLocation(int lane, int stackPosition)
    {
        double x = lane * CoinSize; 
        double y = Height - CoinSize*stackPosition;

        return new(x, y);
    }
}