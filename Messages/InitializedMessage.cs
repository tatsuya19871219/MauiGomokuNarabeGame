using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiGomokuNarabeGame.Messages;

internal class InitializedMessage : ValueChangedMessage<object>
{
    internal InitializedMessage(object value) : base(value)
    {
    }
}
