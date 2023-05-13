using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiGomokuNarabeGame.Messages;

public class FillPoolMessage : ValueChangedMessage<string>
{
    public FillPoolMessage(string value) : base(value)
    {
    }
}
