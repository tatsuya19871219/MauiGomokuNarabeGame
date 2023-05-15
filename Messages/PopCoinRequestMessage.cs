using CommunityToolkit.Mvvm.Messaging.Messages;
using MauiGomokuNarabeGame.Models;

namespace MauiGomokuNarabeGame.Messages;

public class PopCoinRequestMessage : AsyncRequestMessage<Image>
{
    required public Coin RequestCoin { get; init; }
}
