using MauiGomokuNarabeGame.Models;

namespace MauiGomokuNarabeGame;

public class GomokuNarabe
{
    internal Coin NextCoin { get; private set; }
    readonly int _lanes;
    readonly int _stacks;

    internal List<Lane> Lanes = new();
    
    public GomokuNarabe(int lanes, int stacks, Coin firstCoin = Coin.RedCoin)
    {
        _lanes = lanes;
        _stacks = stacks;

        for (int i=0; i<lanes; i++) Lanes.Add( new(i, stacks) );

        NextCoin = firstCoin;
    }

    void PushAt(int laneIndex)
    {
        Lanes[laneIndex].StackCoin(NextCoin);
        NextCoin.Next();
    }

    public bool TryPushAt(int laneIndex)
    {
        if (Lanes[laneIndex].CanStack) 
        {
            PushAt(laneIndex);
            return true;
        }
        else return false;
    }

    public void Reset()
    {
        foreach (var lane in Lanes) lane.Reset();
    }
}
