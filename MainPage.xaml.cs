
using Microsoft.Maui.HotReload;

namespace MauiGomokuNarabeGame;

public partial class MainPage : ContentPage
{
	public MainPage(GomokuNarabeViewModel vm)
	{
		InitializeComponent();

		int lanes = (int)Resources["FieldLanes"];
		int stacks = (int)Resources["FieldStacks"];

		BindingContext = vm.SetFieldSize(lanes, stacks);

		// DoSomething();
	}

	async void DoSomething()
	{
		
		while (true)
		{
			//(GameScreen as IHotReloadableView).Reload();
			await Task.Delay(1000);
		}
	}

}

