﻿using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiGomokuNarabeGame.Messages;

internal class LaneSelectorStateMessage : ValueChangedMessage<int>
{
    internal enum Types
    {
        Show,
        Enable,
        Disable,
        Hide
    }
    
    required internal Types MessageType { get; init; }

    internal LaneSelectorStateMessage(int targetLane) : base(targetLane)
    {
    }
    
}