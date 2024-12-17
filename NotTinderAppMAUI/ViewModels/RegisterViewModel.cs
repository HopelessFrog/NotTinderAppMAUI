using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotTinderAppMAUI.Helprers;
using NotTinderAppMAUI.Models;
using NotTinderAppMAUI.Models.Register;

namespace NotTinderAppMAUI.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly ApiServiceProvider _apiServiceProvider;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string email;

    [ObservableProperty] private bool isBusy;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string login;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string password;

    public RegisterViewModel(ApiServiceProvider apiServiceProvider)
    {
        _apiServiceProvider = apiServiceProvider;
        IsBusy = false;
    }

    private bool CanRegister =>
        !string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password) &&
        !string.IsNullOrWhiteSpace(email);


    private void ClearFields()
    {
        Email = "";
        Login = "";
        Password = "";
    }


    [RelayCommand(CanExecute = nameof(CanRegister))]
    private async Task Register()
    {
        var request = new RegisterRequest
        {
            Email = Email,
            Password = Password,
            Login = Login
        };
        IsBusy = true;
        var response = await _apiServiceProvider.Register(request);
        IsBusy = false;
        if (response.IsSuccess)
        {
            await ToastHelper.ShowToastAsync(response.Message);
            await Shell.Current.GoToAsync("..", true);
            ClearFields();
        }
        else
        {
            await ToastHelper.ShowToastAsync(response.Message);
        }
    }
}