using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiGomokuNarabeGame.Messages;

internal class DisableLaneMessage : ValueChangedMessage<string>
{
    required public int TargetLane { get; init; }

    public DisableLaneMessage(string value) : base(value)
    {
    }
    
}
