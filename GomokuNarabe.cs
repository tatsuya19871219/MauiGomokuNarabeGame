using MauiGomokuNarabeGame.Models;

namespace MauiGomokuNarabeGame;

public class GomokuNarabe
{
    readonly int _lanes;
    readonly int _stacks;

    internal List<Lane> Lanes = new();

    public GomokuNarabe(int lanes, int stacks)
    {
        _lanes = lanes;
        _stacks = stacks;

        for (int i=0; i<lanes; i++) Lanes.Add( new(i, stacks) );
    }
}
