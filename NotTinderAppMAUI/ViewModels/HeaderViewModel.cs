using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NotTinderAppMAUI.Messages;
using NotTinderAppMAUI.Models.User;

namespace NotTinderAppMAUI.ViewModels;

public partial class HeaderViewModel : ObservableObject, IDisposable
{
    private readonly IPopupService _popupService;
    private readonly UserManager _userManager;

    [ObservableProperty] private int _balance;
    [ObservableProperty] private string _name;

    public HeaderViewModel(UserManager userManager, IPopupService popupService)
    {
        _userManager = userManager;
        _popupService = popupService;
        WeakReferenceMessenger.Default.Register<BalanceChangedMessage>(this, (r, m) => { Balance = m.Value; });

        WeakReferenceMessenger.Default.Register<HeaderViewModel, BalanceRequestMessage>(this,
            (r, m) => { m.Reply(r.Balance); });

        Name = Preferences.Get("user_name", string.Empty);
    }

    public bool IsAdmin => Preferences.Get("role", " ") == "Admin";

    public void Dispose()
    {
        WeakReferenceMessenger.Default.Unregister<BalanceChangedMessage>(this);
        WeakReferenceMessenger.Default.Unregister<BalanceRequestMessage>(this);
    }

    [RelayCommand]
    private async Task TopUpBalance()
    {
        await _popupService.ShowPopupAsync<TopUpBalanceViewModel>();
    }

    [RelayCommand]
    private async Task Logout()
    {
        _userManager.LogOut();

        SecureStorage.RemoveAll();
        await Shell.Current.GoToAsync("//LoginPage", true);
    }
}