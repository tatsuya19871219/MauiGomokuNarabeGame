using CommunityToolkit.Mvvm.Messaging;
using MauiGomokuNarabeGame.Messages;
using MauiGomokuNarabeGame.Models;
using System.Runtime.CompilerServices;

namespace MauiGomokuNarabeGame.Views;

public partial class CoinPool : ContentView
{
    #region Bindable properties
    public static readonly BindableProperty CoinImageFilenameProperty
		= BindableProperty.Create(nameof(CoinImageFilename), typeof(string), typeof(CoinPool));
	public static readonly BindableProperty PoolCapacityProperty
		= BindableProperty.Create(nameof(PoolCapacity), typeof(int), typeof(CoinPool));
	public static readonly BindableProperty CoinSizeProperty
		= BindableProperty.Create(nameof(CoinSize), typeof(double), typeof(CoinPool));
	public static readonly BindableProperty PooledCoinProperty
		= BindableProperty.Create(nameof(PooledCoin), typeof(Coin), typeof(CoinPool));

	public string CoinImageFilename
	{
		get => (string)GetValue(CoinImageFilenameProperty);
		set => SetValue(CoinImageFilenameProperty, value);
	}

	public int PoolCapacity
	{
		get => (int)GetValue(PoolCapacityProperty);
		set => SetValue(PoolCapacityProperty, value);
	}

	public double CoinSize
	{
		get => (double)GetValue(CoinSizeProperty);
		set => SetValue(CoinSizeProperty, value);
	}

	public Coin PooledCoin
	{
		get => (Coin)GetValue(PooledCoinProperty);
		set => SetValue(PooledCoinProperty, value);
	}
    #endregion

    Stack<Image> _coinImages = new();

	public CoinPool()
	{
		InitializeComponent();

		StrongReferenceMessenger.Default.Register<PopCoinMessage>(this, (r, m) =>
		{
			if (m.RequestCoin.Equals(PooledCoin)) return;

			var coinImage = _coinImages.Pop();

			Pool.Remove(coinImage);

			m.Reply(coinImage);
		});
	}

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
		switch (propertyName)
		{
			//case nameof(CoinImageFilename):
			//	break;

			case nameof(PoolCapacity):
				FillPool();
				break;

			default:
				base.OnPropertyChanged(propertyName);
				break;
		}

    }

    void FillPool()
	{
		Pool.HorizontalOptions = LayoutOptions.Start;
		Pool.VerticalOptions = LayoutOptions.Start;

		//var coinFactory = (double x, double y) 
		//	=> new Image() { Source = "coin_red.png", WidthRequest = 50, HeightRequest = 50,
		//					TranslationX = x, TranslationY = y};

		for (int i = 0; i < PoolCapacity; i++)
		{
			//var coinImage = coinFactory(0, 0);
			var coinImage = GenerateCoin();

            _coinImages.Push(coinImage);
			Pool.Add(coinImage);
		}
		
	}

	Image GenerateCoin()
	{
		var coin = new Image();

		coin.SetBinding(Image.SourceProperty, new Binding(nameof(CoinImageFilename), source: this));
		coin.SetBinding(Image.WidthRequestProperty, new Binding(nameof(CoinSize), source: this));
		coin.SetBinding(Image.HeightRequestProperty, new Binding(nameof(CoinSize), source: this));

		return coin;
 	}

    private void ContentView_SizeChanged(object sender, EventArgs e)
    {
		if (Width <= 0 || Height <= 0) return;

		UpdateCoinTranslation();
    }

	void UpdateCoinTranslation()
	{
        double x, y;

		double x0 = (Width % CoinSize) / 2;
		double y0 = Height - x0 - CoinSize;

        x = x0;
        y = y0;

        foreach (var coinImage in _coinImages.Reverse())
        {
            coinImage.TranslationX = x;
            coinImage.TranslationY = y;

            x += CoinSize;
            if (x+CoinSize > Width)
            {
                x = x0;
                y -= CoinSize;
            }

        }
    }
}