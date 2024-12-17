using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeviceDetails;
using NotTinderAppMAUI.Helprers;
using NotTinderAppMAUI.Models;
using NotTinderAppMAUI.Models.Authenticate;
using NotTinderAppMAUI.Models.User;

namespace NotTinderAppMAUI.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly ApiServiceProvider _serviceProvider;
    private readonly UserManager _userManager;

    [ObservableProperty] private bool isBusy;


    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(TryLoginCommand))]
    private string login;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(TryLoginCommand))]
    private string password;


    public LoginViewModel(ApiServiceProvider serviceProvider, UserManager userManager)
    {
        _serviceProvider = serviceProvider;
        IsBusy = false;
        _userManager = userManager;
    }

    private bool CanLogin =>
        !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task TryLogin()
    {
        Preferences.Clear();

        var request = new AuthenticateRequest
        {
            DeviceId = new GetDeviceInfo().GetDeviceID(),
            Login = Login,
            Password = Password
        };
        IsBusy = true;
        var response = await _serviceProvider.Authenticate(request);
        IsBusy = false;
        if (response.IsSuccess)
        {
            Preferences.Set("user_id", response.Data.Id);
            Preferences.Set("user_name", response.Data.Name);
            if (response.Data.Roles.Contains("Admin")) Preferences.Set("role", "Admin");


            await Shell.Current.GoToAsync("//MainPage", true).ContinueWith(_ =>
            {
                Login = string.Empty;
                Password = string.Empty;
            });
        }
        else
        {
            await ToastHelper.ShowToastAsync(response.Message);
        }
    }

    [RelayCommand]
    private async Task Register()
    {
        await Shell.Current.GoToAsync("/RegisterPage", true);
    }
}