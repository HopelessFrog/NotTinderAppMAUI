using CommunityToolkit.Mvvm.Messaging.Messages;

namespace NotTinderAppMAUI.Messages;

public class BalanceChangedMessage : ValueChangedMessage<int>
{
    public BalanceChangedMessage(int balance) : base(balance)
    {
    }
}