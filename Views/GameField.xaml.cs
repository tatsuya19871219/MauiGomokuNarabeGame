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
            var targetLane = m.TargetLane;

            //Image coinImage = m.Value;
            Image coinImage = m.CoinImage;

            coinImage.TranslationX = targetLane * CoinSize;
            coinImage.TranslationY = Height - CoinSize*(_coinQueues[targetLane].Count + 1);

            FieldGrid.Add(coinImage);

            _coinQueues[targetLane].Enqueue(coinImage);

            m.Reply(true);

        });

        StrongReferenceMessenger.Default.Register<ClearFieldRequestMessage>(this, async (r, m) =>
        {
            foreach (var queue in _coinQueues)
            {
                RemoveCoinsAsync(queue);
            }

            await Task.Delay(0);

            m.Reply(true);
        });
	}

    async Task RemoveCoinsAsync(Queue<Image> queue)
    {
        while (queue.Count > 0)
        {
            var image = queue.Dequeue();
            
            Task.Run(() => DropCoin(image));
            
            await Task.Delay(100);
        }
    }

    async Task DropCoin(Image image)
    {
        await image.TranslateTo(image.TranslationX, image.TranslationY + Height, 250);
        FieldGrid.Remove(image);
    }

}