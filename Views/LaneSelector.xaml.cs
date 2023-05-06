using CommunityToolkit.Mvvm.Input;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiGomokuNarabeGame.Views;

public partial class LaneSelector : ContentView
{
    //public static readonly BindableProperty CoinSizeProperty =
    //	BindableProperty.Create(nameof(CoinSize), typeof(int), typeof(LaneSelector), 0);
    //required public ICommand SelectCommand { get; init; }

    public static readonly BindableProperty SelectCommandProperty =
        BindableProperty.Create(nameof(SelectCommand), typeof(ICommand), typeof(LaneSelector));

    public ICommand SelectCommand
    {
        get => (ICommand)GetValue(SelectCommandProperty);
        set => SetValue(SelectCommandProperty, value);
    }

    //public static readonly BindableProperty SelectCommandProperty =
    //    BindableProperty.Create(nameof(SelectCommand), typeof(RelayCommand), typeof(LaneSelector));

    //public RelayCommand SelectCommand
    //{
    //    get => (RelayCommand)GetValue(SelectCommandProperty);
    //    set => SetValue(SelectCommandProperty, value);
    //}

    //public int CoinSize { get; set; }
    public LaneSelector()
	{
		InitializeComponent();

		BindingContext = this;
	}


	protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		switch (propertyName)
		{
			case nameof(SelectCommand):
                //SelectCommand.Execute(this);
				break;

            default:
		        base.OnPropertyChanged(propertyName);
                break;
		}

	}

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        //SelectCommand.Execute(this);
    }
}