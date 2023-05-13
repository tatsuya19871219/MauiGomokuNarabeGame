using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiGomokuNarabeGame.Messages;

public class ClearFieldMessage : ValueChangedMessage<string>
{
    public ClearFieldMessage(string value) : base(value)
    {
    }
}
