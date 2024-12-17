using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotTinderAppMAUI.Models.User;

namespace NotTinderAppMAUI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly UserManager _userManager;

    public MainViewModel(UserManager userManager)
    {
        _userManager = userManager;
        // _achivmentsView = achievementsView;
    }

    [RelayCommand]
    private async Task LogOut()
    {
        _userManager.LogOut();

        await Shell.Current.GoToAsync("//LoginPage", true);
    }
}