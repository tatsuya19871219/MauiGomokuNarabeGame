using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiGomokuNarabeGame.Models;

public partial class Lane : ObservableObject
{
    public int LaneIndex { get; private set; }
    //public bool CanStack { get; private set; }
    [ObservableProperty] bool _canStack;

    readonly int _stacks;
    public List<Coin> StackedCoins { get; init; } = new();
    public int CurrentPosition => StackedCoins.Count();

    public Lane(int laneIndex, int stacks)
    {
        LaneIndex = laneIndex;
        CanStack = true;

        _stacks = stacks;
    }

    public void StackCoin(Coin coin)
    {
        if (CanStack) StackedCoins.Add(coin);

        if (CurrentPosition == _stacks) CanStack = false;
    }
    public void Reset()
    {
        StackedCoins.Clear();
        CanStack = true;
    }
    
}
