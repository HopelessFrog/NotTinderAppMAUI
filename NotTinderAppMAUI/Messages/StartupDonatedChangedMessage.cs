using CommunityToolkit.Mvvm.Messaging.Messages;

namespace NotTinderAppMAUI.Messages;

public class StartupDonatedChangedMessage : ValueChangedMessage<int>
{
    public StartupDonatedChangedMessage(int value) : base(value)
    {
    }
}