using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiGomokuNarabeGame.Messages;

internal class InitializingMessage : ValueChangedMessage<object>
{
    public InitializingMessage(object value) : base(value)
    {
    }
}
