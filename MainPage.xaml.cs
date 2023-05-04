namespace MauiGomokuNarabeGame;

public partial class MainPage : ContentPage
{
	public MainPage(GomokuNarabeViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}

}

