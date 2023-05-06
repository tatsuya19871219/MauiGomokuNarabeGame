using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiGomokuNarabeGame.Models;

public partial class Lane : ObservableObject
{
    [ObservableProperty] int _laneIndex;
    readonly int _stacks;

    public Lane(int laneIndex, int stacks)
    {
        LaneIndex = laneIndex;
        _stacks = stacks;
    }
}
