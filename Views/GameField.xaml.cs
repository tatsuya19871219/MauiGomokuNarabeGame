using System.Collections.Concurrent;
using CommunityToolkit.Mvvm.Messaging;
using MauiGomokuNarabeGame.Helpers;
using MauiGomokuNarabeGame.Messages;

namespace MauiGomokuNarabeGame.Views;

public partial class GameField : ContentView
{
    #region Properties
    public static readonly BindableProperty StacksProperty
        = BindableProperty.Create(nameof(Stacks), typeof(int), typeof(GameField));
    public static readonly BindableProperty CoinSizeProperty
        = BindableProperty.Create(nameof(CoinSize), typeof(double), typeof(CoinPool));


    public int Stacks
    {
        get => (int)GetValue(StacksProperty);
        set
        {
            SetValue(StacksProperty, value);
        }
    }

    public double CoinSize
    {
        get => (double)GetValue(CoinSizeProperty);
        set => SetValue(CoinSizeProperty, value);
    }
    
    readonly int _lanes;
    required public int Lanes
    {
        get => _lanes;
        init
        {
            _lanes = value;
            for (int i = 0; i < _lanes; i++) _coinQueues.Add(new());
        }
    }

    #endregion

    List<Queue<Image>> _coinQueues = new();

    public GameField()
	{
		InitializeComponent();

        WeakReferenceMessenger.Default.Send(new InitializingMessage(this));

        new ConditionalAction(
                action: () => 
                {
                    WeakReferenceMessenger.Default.Send(new InitializedMessage(this));
                },
                () => new[] {Stacks, Lanes}.All(value => value > 0)
            ){ MillisecondsTimeout = 100 }.Invoke();


        StrongReferenceMessenger.Default.Register<InsertCoinRequestMessage>(this, (r, m) =>
        {
            Image coinImage = m.CoinImage;
            var targetLane = m.TargetLane;

            _coinQueues[targetLane].Enqueue(coinImage);

            var stackPosition = _coinQueues[targetLane].Count;
            
            m.Reply(InsertCoinAsync(coinImage, targetLane, stackPosition));

        });

        StrongReferenceMessenger.Default.Register<ClearFieldRequestMessage>(this, async (r, m) =>
        {
            m.Reply(RemoveCoinQueuesAsync(_coinQueues));
        });
	}

    async Task<bool> InsertCoinAsync(Image coinImage, int targetLane, int stackPosition)
    {
        double insertSpeedRatio = 3.0;

        var x0 = targetLane * CoinSize;
        var y0 = -100.0;

        var x1 = x0;
        var y1 = Height - CoinSize*stackPosition;

        var length = (uint) (Math.Abs(y1-y0)/insertSpeedRatio);

        coinImage.TranslationX = x0;
        coinImage.TranslationY = y0;
        
        FieldGrid.Add(coinImage);

        await coinImage.TranslateTo(x1, y1, length);

        return true;
    }

    async Task<bool> RemoveCoinQueuesAsync(List<Queue<Image>> queues)
    {
        var results = new ConcurrentBag<bool>();

        Parallel.ForEach(queues, async queue =>
        {
            results.Add( await RemoveCoinsAsync(queue) );

            queue.Clear();
        });

        return results.All(r=>r);
    }

    async Task<bool> RemoveCoinsAsync(Queue<Image> queue)
    {
        while (queue.Count > 0)
        {
            var image = queue.Dequeue();
            
            await DropCoin(image);
            
            await Task.Delay(100);
        }

        return queue.Count == 0;
    }

    async Task DropCoin(Image image)
    {
        await image.TranslateTo(image.TranslationX, image.TranslationY + Height, 250);
    }

}