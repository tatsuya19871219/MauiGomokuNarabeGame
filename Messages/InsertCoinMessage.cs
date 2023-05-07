using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiGomokuNarabeGame.Messages;

internal class InsertCoinMessage : ValueChangedMessage<Image>
{
    required public int TargetLane { get; init; }

    public InsertCoinMessage(Image value) : base(value)
    {
    }
}
