using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NotTinderAppMAUI.Helprers;
using NotTinderAppMAUI.Messages;
using NotTinderAppMAUI.Models;
using NotTinderAppMAUI.Models.DTOs;

namespace NotTinderAppMAUI.ViewModels;

public partial class TopStartupsViewModel : ObservableObject
{
    private readonly ApiServiceProvider _apiServiceProvider;
    private readonly IPopupService _popupService;
    [ObservableProperty] private bool _isRefreshing;

    [ObservableProperty] private ObservableCollection<TopStartupDto> _startups = new();

    public TopStartupsViewModel(IPopupService popupService, ApiServiceProvider apiServiceProvider)
    {
        _popupService = popupService;
        _apiServiceProvider = apiServiceProvider;


        WeakReferenceMessenger.Default.Register<StartupDonatedChangedMessage>(this, async (r, m) =>
        {
            var startup = Startups.FirstOrDefault(s => s.Id == m.Value);
            if (startup != null)
            {
                var response = await _apiServiceProvider.CallWebApi<int>($"/api/Donated?id={startup.Id}");
                if (response.IsSuccess) startup.Donated = response.Data;
            }
        });

        Task.Run(async () => await LoadStartuos());
    }

    [RelayCommand]
    private async Task LoadStartuos()
    {
        var startupsResponse = await _apiServiceProvider.CallWebApi<List<TopStartupDto>>("/api/TopStartups");
        if (startupsResponse.IsSuccess)
        {
            Startups.Clear();
            if (startupsResponse?.Data == null)
            {
                IsRefreshing = false;
                return;
            }

            foreach (var startup in startupsResponse.Data) Startups.Add(startup);
        }
        else
        {
            await Task.Delay(500);
        }

        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task OpenStartup(TopStartupDto startup)
    {
        var response = await _apiServiceProvider.CallWebApi<StartupShort>($"/api/Startups?startupId={startup.Id}");
        if (!response.IsSuccess)
        {
            await ToastHelper.ShowToastAsync("Unable to open startup");
            return;
        }

        await _popupService.ShowPopupAsync<StartupViewModel>(async viewModel =>
        {
            viewModel.Startup = response.Data;
            viewModel.Startup.Donated = startup.Donated;
            viewModel.LoadDonatersCommand.Execute(null);
            viewModel.CanDonate = response.Data.OwnerId != Preferences.Get("user_id", string.Empty);
        });
    }
}