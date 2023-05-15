using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiGomokuNarabeGame.Messages;

internal class InsertCoinRequestMessage : AsyncRequestMessage<bool>
{
    required internal Image CoinImage { get; init; }
    required internal int TargetLane { get; init; }
}
