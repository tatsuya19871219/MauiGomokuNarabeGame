using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiGomokuNarabeGame.Messages;

internal class InitializedMessage : ValueChangedMessage<object>
{
    public InitializedMessage(object value) : base(value)
    {
    }
}
