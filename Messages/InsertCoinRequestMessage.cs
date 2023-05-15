using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiGomokuNarabeGame.Messages;

internal class InsertCoinRequestMessage : AsyncRequestMessage<bool>
{
    required public int TargetLane { get; init; }
    required public Image CoinImage { get; init; }

}
