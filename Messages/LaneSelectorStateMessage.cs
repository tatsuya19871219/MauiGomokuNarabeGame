using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiGomokuNarabeGame.Messages;

internal class LaneSelectorStateMessage : ValueChangedMessage<int>
{
    public enum Types
    {
        Show,
        Enable,
        Disable,
        Hide
    }
    //required public int TargetLane { get; init; }

    required public Types MessageType { get; init; }

    public LaneSelectorStateMessage(int targetLane) : base(targetLane)
    {
    }
    
}
