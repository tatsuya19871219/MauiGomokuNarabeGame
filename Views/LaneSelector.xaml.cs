using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using MauiGomokuNarabeGame.Helpers;
using MauiGomokuNarabeGame.Messages;

namespace MauiGomokuNarabeGame.Views;

public partial class LaneSelector : ContentView
{
    #region Properties
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
    
    #endregion    

    bool _isSummoning = false;

    readonly OnceAtTimeAction _selectorAnimation;
    readonly OnceAtTimeAction _disabledMarkAnimation;

    public LaneSelector()
    {
        InitializeComponent();

        VisualStateManager.GoToState(LaneSelectorGrid, "Enable");

        WeakReferenceMessenger.Default.Register<LaneSelectorEnableMessage>(this, (r, m) =>
        {
            if (m.TargetLane is int targetLane && targetLane != LaneIndex) return;

            var enable = m.Value;

            VisualStateManager.GoToState(LaneSelectorGrid, enable ? "Enable" : "Disable");
        });

        WeakReferenceMessenger.Default.Register<LaneSelectorVisibleMessage>(this, (r, m) =>
        {
            if (m.TargetLane is int targetLane && targetLane != LaneIndex) return;
            if (_isSummoning) return;

            var visible = m.Value;

            VisualStateManager.GoToState(this, visible ? "Show" : "Hide");
        });

        _selectorAnimation = new(SelectorFade);
        _disabledMarkAnimation = new(DisabledMarkShake);

    }

    private async void Selector_Tapped(object sender, TappedEventArgs e)
    {
        VisualStateManager.GoToState(LaneSelectorGrid, "Summoning");
        _isSummoning = true;

        SelectCommand.Execute(LaneIndex);

        await _selectorAnimation.TryInvokeAsync();

        _isSummoning = false;
        VisualStateManager.GoToState(LaneSelectorGrid, "Enable");
    }

    private async void DisabledMark_Tapped(object sender, TappedEventArgs e)
    {
        await _disabledMarkAnimation.TryInvokeAsync();
    }

    async Task SelectorFade()
    {
        var element = SelectorArrow as VisualElement;

        var x = element.TranslationX;
        var y = element.TranslationY;

        _ = element.ScaleTo(1.2, 100).ContinueWith((_) => element.ScaleTo(1, 100));
        _ = element.TranslateTo(x, y - 25);
        await element.FadeTo(0);

        VisualStateManager.GoToState(this, "Hide");

        element.Opacity = 1;
        element.TranslationX = x;
        element.TranslationY = y;
    }

    async Task DisabledMarkShake()
    {
        var element = DisabledMark as VisualElement;

        _ = element.ScaleTo(1.2, 100).ContinueWith((_) => element.ScaleTo(1, 100));
        await element.RelRotateTo(-30, 50);
        await element.RelRotateTo(60, 50);
        await element.RelRotateTo(-30, 50);
    }

}