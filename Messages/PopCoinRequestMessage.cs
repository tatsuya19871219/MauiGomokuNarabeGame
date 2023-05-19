using CommunityToolkit.Mvvm.Messaging.Messages;

using MauiGomokuNarabeGame.Models;

namespace MauiGomokuNarabeGame.Messages;

internal class PopCoinRequestMessage : AsyncRequestMessage<Image>
{
    required internal Coin RequestCoin { get; init; }
}
