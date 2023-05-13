using CommunityToolkit.Mvvm.Messaging;
using MauiGomokuNarabeGame.Messages;
using System.Runtime.CompilerServices;

namespace MauiGomokuNarabeGame.Views;

public partial class GameField : ContentView
{
    #region Bindable Properties
    public static readonly BindableProperty LanesProperty
        = BindableProperty.Create(nameof(Lanes), typeof(int), typeof(GameField));
    public static readonly BindableProperty StacksProperty
        = BindableProperty.Create(nameof(Stacks), typeof(int), typeof(GameField));
    public static readonly BindableProperty CoinSizeProperty
        = BindableProperty.Create(nameof(CoinSize), typeof(double), typeof(CoinPool));

    public int Lanes
    {
        get => (int)GetValue(LanesProperty);
        set => SetValue(LanesProperty, value);
    }

    public int Stacks
    {
        get => (int)GetValue(StacksProperty);
        set => SetValue(StacksProperty, value);
    }

    public double CoinSize
    {
        get => (double)GetValue(CoinSizeProperty);
        set => SetValue(CoinSizeProperty, value);
    }
    #endregion

    List<Queue<Image>> _coinQueues = new();
    //List<int> _stackPosition = new();

    public GameField()
	{
		InitializeComponent();

        //FieldGrid.HorizontalOptions = LayoutOptions.Start;
        //FieldGrid.VerticalOptions = LayoutOptions.Start;

        StrongReferenceMessenger.Default.Register<InsertCoinMessage>(this, (r, m) =>
        {
            var targetLane = m.TargetLane;

            Image coinImage = m.Value;

            coinImage.TranslationX = targetLane * CoinSize;
            coinImage.TranslationY = Height - CoinSize*(_coinQueues[targetLane].Count + 1);

            FieldGrid.Add(coinImage);

            _coinQueues[targetLane].Enqueue(coinImage);

        });

        StrongReferenceMessenger.Default.Register<ClearFieldMessage>(this, async (r, m) =>
        {
            foreach (var queue in _coinQueues)
            {
                RemoveCoinsAsync(queue);
            }

            await Task.Delay(0);
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

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        switch (propertyName)
        {
            case nameof(Lanes):
                
                _coinQueues.Clear();
                for (int i = 0; i < Lanes; i++) _coinQueues.Add(new());
                                                
                break;

            default:
                base.OnPropertyChanged(propertyName);
                break;
        }
        
    }
}