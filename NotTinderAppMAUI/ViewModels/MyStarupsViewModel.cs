using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NotTinderAppMAUI.Helprers;
using NotTinderAppMAUI.Messages;
using NotTinderAppMAUI.Models;
using NotTinderAppMAUI.Services.Interfaces;

namespace NotTinderAppMAUI.ViewModels;

public partial class MyStarupsViewModel : ObservableObject, IDisposable
{
    private readonly ApiServiceProvider _apiServiceProvider;
    private readonly IPopupService _popupService;
    [ObservableProperty] private bool _isRefreshing;

    [ObservableProperty] private ObservableCollection<StartupShort> _startups = new();

    public MyStarupsViewModel(IPopupService popupService, ApiServiceProvider apiServiceProvider,
        IBalanceService balanceService)
    {
        _popupService = popupService;
        _apiServiceProvider = apiServiceProvider;

        Task.Run(async () => await balanceService.UpdateBalance());


        Task.Run(async () => await LoadStartuos());
    }

    public void Dispose()
    {
        WeakReferenceMessenger.Default.Unregister<StartupDonatedChangedMessage>(this);
    }

    [RelayCommand]
    private async Task LoadStartuos()
    {
        var startupsResponse = await _apiServiceProvider.CallWebApi<List<StartupShort>>("/api/MyStartups");
        if (startupsResponse.IsSuccess)
        {
            Startups.Clear();
            if (startupsResponse?.Data == null)
            {
                IsRefreshing = false;
                return;
            }

            foreach (var startup in startupsResponse.Data)
            {
                var donatedResponse = await _apiServiceProvider.CallWebApi<int>($"/api/Donated?id={startup.Id}");
                if (donatedResponse.IsSuccess)
                    startup.Donated = donatedResponse.Data;
                else
                    startup.Donated = 0;

                Startups.Add(startup);
            }
        }
        else
        {
            await Task.Delay(500);
        }

        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task AddStartup()
    {
        var result = await _popupService.ShowPopupAsync<AddStartupViewModel>();

        if (result is bool boolResult)
            if (boolResult)
                await LoadStartuos();
    }

    [RelayCommand]
    private async Task EditStartup(StartupShort startup)
    {
        var result =
            await _popupService.ShowPopupAsync<UpdateStartupViewModel>(viewModel =>
                viewModel.LoadStartupData(startup));

        if (result is bool boolResult)
            if (boolResult)
                await LoadStartuos();
    }

    [RelayCommand]
    private async Task OpenStartup(StartupShort startup)
    {
        await _popupService.ShowPopupAsync<StartupViewModel>(viewModel =>
        {
            viewModel.Startup = startup;
            viewModel.LoadDonatersCommand.Execute(null);
            viewModel.CanDonate = startup.OwnerId != Preferences.Get("user_id", string.Empty);
        });
    }

    [RelayCommand]
    private async Task DeleteStartup(StartupShort startup)
    {
        var deleteResponse =
            await _apiServiceProvider.CallWebApi($"/api/Startups?id={startup.Id}", method: HttpMethod.Delete);
        if (deleteResponse.IsSuccess)
        {
            await ToastHelper.ShowToastAsync("Startup deleted");
            await LoadStartuos();
        }
        else
        {
            await ToastHelper.ShowToastAsync("Error deleting Startup");
        }
    }
}