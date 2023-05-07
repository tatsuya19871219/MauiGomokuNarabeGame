using CommunityToolkit.Mvvm.Messaging.Messages;
using MauiGomokuNarabeGame.Models;

namespace MauiGomokuNarabeGame.Messages;

public class PopCoinMessage : RequestMessage<Image>
{
    required public Coin RequestCoin { get; init; }
}
