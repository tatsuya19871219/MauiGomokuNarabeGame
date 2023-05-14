using CommunityToolkit.Mvvm.Messaging.Messages;
using MauiGomokuNarabeGame.Models;

namespace MauiGomokuNarabeGame.Messages;

public class FillPoolMessage : ValueChangedMessage<string>
{
    //required public Coin PooledCoin { get; init; }
    public FillPoolMessage(string value) : base(value)
    {
    }
}
