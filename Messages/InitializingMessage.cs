using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiGomokuNarabeGame.Messages;

internal class InitializingMessage : ValueChangedMessage<object>
{
    internal InitializingMessage(object value) : base(value)
    {
    }
}
