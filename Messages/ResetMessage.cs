using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiGomokuNarabeGame.Messages;

public class ResetMessage : ValueChangedMessage<string>
{
    public ResetMessage(string value) : base(value)
    {
    }
}
