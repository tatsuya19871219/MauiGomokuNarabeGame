using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiGomokuNarabeGame.Messages;

internal class LaneSelectorEnableMessage : ValueChangedMessage<bool>
{
    internal int? TargetLane { get; init; } 
    internal LaneSelectorEnableMessage(bool enable) : base(enable)
    {
    }
}