namespace MauiGomokuNarabeGame;

public partial class MainPage : ContentPage
{
	public MainPage(GomokuNarabeViewModel vm)
	{
		InitializeComponent();

		int lanes = (int)Resources["FieldLanes"];
		int stacks = (int)Resources["FieldStacks"];

		BindingContext = vm.SetFieldSize(lanes, stacks);		
	}
}

