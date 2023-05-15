using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using MauiGomokuNarabeGame.Messages;

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

        VisualStateManager.GoToState(this, "Enable");

        WeakReferenceMessenger.Default.Register<LaneSelectorStateMessage>(this, (r, m) =>
        {
            if (m.Value != LaneIndex) return;

            var state = m.MessageType switch 
            {
                LaneSelectorStateMessage.Types.Show => "Show",
                LaneSelectorStateMessage.Types.Enable => "Enable",
                LaneSelectorStateMessage.Types.Disable => "Disable",
                LaneSelectorStateMessage.Types.Hide => "Hide",
                _ => throw new Exception("Unsupported message")
            };

            VisualStateManager.GoToState(this, state);
        });

	}

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        SelectCommand.Execute(LaneIndex);
    }
}