using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiGomokuNarabeGame.Messages;

internal class LaneSelectorVisibleMessage : ValueChangedMessage<bool>
{
    internal int? TargetLane { get; init; }
    internal LaneSelectorVisibleMessage(bool visible) : base(visible)
    {
    }
}