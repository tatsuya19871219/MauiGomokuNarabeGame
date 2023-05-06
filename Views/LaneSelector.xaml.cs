using System.Windows.Input;

namespace MauiGomokuNarabeGame.Views;

public partial class LaneSelector : ContentView
{
    public static readonly BindableProperty SelectCommandProperty =
        BindableProperty.Create(nameof(SelectCommand), typeof(ICommand), typeof(LaneSelector));
    public static readonly BindableProperty LaneIndexProperty =
        BindableProperty.Create(nameof(LaneIndex), typeof(int), typeof(LaneSelector));

    public ICommand SelectCommand
    {
        get => (ICommand)GetValue(SelectCommandProperty);
        set => SetValue(SelectCommandProperty, value);
    }

    public int LaneIndex
    {
        get => (int)GetValue(LaneIndexProperty);
        set => SetValue(LaneIndexProperty, value);
    }

    public LaneSelector()
	{
		InitializeComponent();

		BindingContext = this;
	}


	// protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
	// {
	// 	switch (propertyName)
	// 	{
	// 		case nameof(SelectCommand):
    //             //SelectCommand.Execute(this);
	// 			break;

    //         default:
	// 	        base.OnPropertyChanged(propertyName);
    //             break;
	// 	}

	// }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        SelectCommand.Execute(LaneIndex);
    }
}