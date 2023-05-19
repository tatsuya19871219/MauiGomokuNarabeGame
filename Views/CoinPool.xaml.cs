using CommunityToolkit.Mvvm.Messaging;
using MauiGomokuNarabeGame.Helpers;
using MauiGomokuNarabeGame.Messages;
using MauiGomokuNarabeGame.Models;

namespace MauiGomokuNarabeGame.Views;

public partial class CoinPool : ContentView
{
    #region Properties    
	public static readonly BindableProperty CoinSizeProperty
		= BindableProperty.Create(nameof(CoinSize), typeof(double), typeof(CoinPool));
	public double CoinSize
	{
		get => (double)GetValue(CoinSizeProperty);
		set => SetValue(CoinSizeProperty, value);
	}

	required public string CoinImageFilename { get; init; }
	required public Coin PooledCoin { get; init; }
	required public int PoolCapacity { get; init; }

    #endregion

    Stack<Image> _coinImages = new();

	bool _isFilling = false;

	public CoinPool()
	{
		InitializeComponent();

		WeakReferenceMessenger.Default.Send(new InitializingMessage(this));

		new ConditionalAction(
				action: () => 
				{
					FillPool();
					WeakReferenceMessenger.Default.Send(new InitializedMessage(this));
				},
				() => PooledCoin is not Coin.NullCoin,
				() => PoolCapacity > 0
			).Invoke();

		WeakReferenceMessenger.Default.Register<PopCoinRequestMessage>(this, (r, m) =>
		{
			if (!m.RequestCoin.Equals(PooledCoin)) return;

			m.Reply( PopCoinAsync() );
		});

		StrongReferenceMessenger.Default.Register<FillPoolRequestMessage>(this, (r, m) =>
		{
			m.Reply( FillPoolAsync() );
		});
	}

	async Task<Image> PopCoinAsync()
	{
		var coinImage = _coinImages.Pop();

		var x = coinImage.TranslationX;
		var y = coinImage.TranslationY;

		_ = coinImage.TranslateTo(x, y-50);
		await coinImage.FadeTo(0);
			
		Pool.Remove(coinImage);

		coinImage.Opacity = 1;

		return coinImage;
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

    async Task<bool> FillPoolAsync()
	{
		if (_isFilling) throw new Exception("I'm already filling");

		_isFilling = true;

		var coinPositionEnumerator = CoinPositionGenerator().GetEnumerator();

		for (int i = 0; i < PoolCapacity; i++)
		{
			coinPositionEnumerator.MoveNext();
			if (i < _coinImages.Count) continue;

			var (x, y) = coinPositionEnumerator.Current;

			var coinImage = GenerateCoin(x, y);

			coinImage.TranslationY = -100;

			var animationTime = (uint)Math.Abs(Math.Round(y/50));

			_ = coinImage.TranslateTo(x, y, animationTime);
			
			await Task.Delay(100);

			_coinImages.Push(coinImage);

			Pool.Add(coinImage);
		}

		var coinCounts = _coinImages.Count;

		_isFilling = false;

		return coinCounts == PoolCapacity;
		
	}

	Image GenerateCoin(double x = 0, double y = 0)
	{
		var coin = new CoinImage(CoinImageFilename) { TranslationX = x, TranslationY = y };

		return coin;
 	}

    private void ContentView_SizeChanged(object sender, EventArgs e)
    {
		if (Width <= 0 || Height <= 0) return;

		UpdateCoinTranslation();
    }

	void UpdateCoinTranslation()
	{
		if (CoinSize <= 0) return;

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