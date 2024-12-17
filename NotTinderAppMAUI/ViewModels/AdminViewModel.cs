using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotTinderAppMAUI.Models;
using NotTinderAppMAUI.Models.DTOs;

namespace NotTinderAppMAUI.ViewModels;

public partial class AdminViewModel : ObservableObject
{
    private readonly ApiServiceProvider _apiServiceProvider;
    private readonly IPopupService _popupService;

    [ObservableProperty] private bool _isRefreshing;

    [ObservableProperty] private ObservableCollection<UserData> _users = new();

    public AdminViewModel(ApiServiceProvider apiServiceProvider, IPopupService popupService)
    {
        _apiServiceProvider = apiServiceProvider;
        _popupService = popupService;

        Task.Run(async () => LoadUsers());
    }

    [RelayCommand]
    private async Task LoadUsers()
    {
        var usersResponse = await _apiServiceProvider.CallWebApi<List<UserData>>("/api/AllUsers");
        if (usersResponse.IsSuccess)
        {
            Users.Clear();
            if (usersResponse?.Data == null)
            {
                IsRefreshing = false;
                return;
            }

            foreach (var user in usersResponse.Data) Users.Add(user);
        }
        else
        {
            await Task.Delay(500);
        }

        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task OpenTransactionsStat(UserData user)
    {
        await _popupService.ShowPopupAsync<TransactionsViewModel>(viewmodel =>
        {
            viewmodel.UserId = user.Id;
            viewmodel.LoadTransactionsCommand.Execute(null);
        });
    }
}