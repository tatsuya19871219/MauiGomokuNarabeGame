using CommunityToolkit.Mvvm.Messaging;
using MauiGomokuNarabeGame.Messages;
using MauiGomokuNarabeGame.Models;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiGomokuNarabeGame.Views;

public partial class CoinPool : ContentView
{
    #region Bindable properties
    public static readonly BindableProperty CoinImageFilenameProperty
		= BindableProperty.Create(nameof(CoinImageFilename), typeof(string), typeof(CoinPool));
	// public static readonly BindableProperty PoolCapacityProperty
	// 	= BindableProperty.Create(nameof(PoolCapacity), typeof(int), typeof(CoinPool));
	public static readonly BindableProperty CoinSizeProperty
		= BindableProperty.Create(nameof(CoinSize), typeof(double), typeof(CoinPool));
	public static readonly BindableProperty PooledCoinProperty
		= BindableProperty.Create(nameof(PooledCoin), typeof(Coin), typeof(CoinPool));

	public string CoinImageFilename
	{
		get => (string)GetValue(CoinImageFilenameProperty);
		set => SetValue(CoinImageFilenameProperty, value);
	}

	// public int PoolCapacity
	// {
	// 	get => (int)GetValue(PoolCapacityProperty);
	// 	set => SetValue(PoolCapacityProperty, value);
	// }
	readonly int _poolCapacity;

	required public int PoolCapacity 
	{ 
		get => _poolCapacity;
		init
		{
			_poolCapacity = value;
			FillPool();
		}
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

	public static readonly BindableProperty OnReadyCommandProperty =
		BindableProperty.Create(nameof(OnReadyCommand), typeof(ICommand), typeof(CoinPool));
	public ICommand OnReadyCommand
	{
		get => (ICommand)GetValue(OnReadyCommandProperty);
		set => SetValue(OnReadyCommandProperty, value);
	}
    #endregion

    Stack<Image> _coinImages = new();

	public CoinPool()
	{
		InitializeComponent();

		StrongReferenceMessenger.Default.Register<PopCoinMessage>(this, (r, m) =>
		{
			if (!m.RequestCoin.Equals(PooledCoin)) return;

			var coinImage = _coinImages.Pop();

			Pool.Remove(coinImage);

			m.Reply(coinImage);
		});

		StrongReferenceMessenger.Default.Register<FillPoolMessage>(this, (r, m) =>
		{
			FillPoolAsync();			
		});
	}

	void FillPool()
	{
		for (int i = _coinImages.Count; i < PoolCapacity; i++)
		{
			var coinImage = GenerateCoin();

            _coinImages.Push(coinImage);
			Pool.Add(coinImage);
		}

		if (CoinSize > 0) UpdateCoinTranslation();
	}

    async void FillPoolAsync()
	{
		var coinPositionEnumerator = CoinPositionGenerator().GetEnumerator();

		for (int i = 0; i < PoolCapacity; i++)
		{
			coinPositionEnumerator.MoveNext();
			if (i < _coinImages.Count) continue;

			var (x, y) = coinPositionEnumerator.Current;

			var coinImage = GenerateCoin(x, y);

			coinImage.TranslationY = -100;

			var animationTime = (uint)Math.Abs(Math.Round(y/50));

			_ = Task.Run(() => coinImage.TranslateTo(x, y, animationTime));
			
			await Task.Delay(100);

			_coinImages.Push(coinImage);

			Pool.Add(coinImage);
		}
		
	}

	Image GenerateCoin(double x = 0, double y = 0)
	{
		var coin = new Image() { TranslationX = x, TranslationY = y };

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
		var coinPositionEnumerator = CoinPositionGenerator().GetEnumerator();

        foreach (var coinImage in _coinImages.Reverse())
        {
			coinPositionEnumerator.MoveNext();
			var (x, y) = coinPositionEnumerator.Current;

            coinImage.TranslationX = x;
            coinImage.TranslationY = y;
        }
    }

	IEnumerable<Point> CoinPositionGenerator()
	{
		if (PoolCapacity <= 0) throw new Exception("Invalid pool capacity.");
		if (CoinSize <= 0) throw new Exception("Invalid coin size.");
		if (Width <= 0 || Height <= 0) throw new Exception("Invalid width/height of view");

		double x, y;

        double x0 = (Width % CoinSize) / 2;
        double y0 = Height - x0 - CoinSize;

        x = x0;
        y = y0;

		yield return new(x, y);

        for (int i=1; i < PoolCapacity; i++)
		{
			x += CoinSize;
            if (x+CoinSize > Width)
            {
                x = x0;
                y -= CoinSize;
            }

			yield return new(x, y);
		}
	}
}