using CommunityToolkit.Mvvm.Messaging.Messages;
using MauiGomokuNarabeGame.Models;

namespace MauiGomokuNarabeGame.Messages;

public class PopCoinMessage : AsyncRequestMessage<Image>
{
    required public Coin RequestCoin { get; init; }
}
