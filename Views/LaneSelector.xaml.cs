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

    bool _isSummoning = false;

    public LaneSelector()
	{
		InitializeComponent();

        VisualStateManager.GoToState(LaneSelectorGrid, "Enable");

        // WeakReferenceMessenger.Default.Register<LaneSelectorStateMessage>(this, (r, m) =>
        // {
        //     if (m.Value != LaneIndex) return;

        //     var state = m.MessageType switch 
        //     {
        //         LaneSelectorStateMessage.Types.Show => "Show",
        //         LaneSelectorStateMessage.Types.Enable => "Enable",
        //         LaneSelectorStateMessage.Types.Disable => "Disable",
        //         LaneSelectorStateMessage.Types.Hide => "Hide",
        //         _ => throw new Exception("Unsupported message")
        //     };

        //     VisualStateManager.GoToState(this, state);
        // });

        WeakReferenceMessenger.Default.Register<LaneSelectorEnableMessage>(this, (r, m) =>
        {
            if (m.TargetLane is int targetLane && targetLane != LaneIndex) return;

            var enable = m.Value;

            VisualStateManager.GoToState(LaneSelectorGrid, enable ? "Enable" : "Disable");
            //VisualStateManager.GoToState(LaneSelectorGrid, "Disable");
        });

        WeakReferenceMessenger.Default.Register<LaneSelectorVisibleMessage>(this, (r, m) =>
        {
            if (m.TargetLane is int targetLane && targetLane != LaneIndex) return;
            if (_isSummoning) return;

            var visible = m.Value;

            VisualStateManager.GoToState(this, visible ? "Show" : "Hide");
        });

	}

    private async void Selector_Tapped(object sender, TappedEventArgs e)
    {
        _isSummoning = true;

        SelectCommand.Execute(LaneIndex);

        //if (SelectorArrow.IsAnimationPlaying) return;

        var element = sender as VisualElement;

        var x = element.TranslationX;
        var y = element.TranslationY;

        _ = element.ScaleTo(1.2, 100).ContinueWith((_) => element.ScaleTo(1, 100));
        _ = element.TranslateTo(x, y - 25);
        await element.FadeTo(0);

        VisualStateManager.GoToState(this, "Hide");

        _isSummoning = false;

        element.Opacity = 1;
        element.TranslationX = x;
        element.TranslationY = y;
    }

    private async void DisabledMark_Tapped(object sender, TappedEventArgs e)
    {
        //if (DisabledMark.IsAnimationPlaying) return;

        var element = sender as VisualElement;

        _ = element.ScaleTo(1.2, 100).ContinueWith((_) => element.ScaleTo(1, 100));
        await element.RelRotateTo(-30, 50);
        await element.RelRotateTo(60, 50);
        await element.RelRotateTo(-30, 50);
    }
}