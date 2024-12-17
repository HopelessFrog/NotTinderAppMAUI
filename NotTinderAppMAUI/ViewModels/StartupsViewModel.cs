using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NotTinderAppMAUI.Helprers;
using NotTinderAppMAUI.Messages;
using NotTinderAppMAUI.Models;

namespace NotTinderAppMAUI.ViewModels;

public partial class StartupsViewModel : ObservableObject, IDisposable
{
    private readonly ApiServiceProvider _apiServiceProvider;
    private readonly IPopupService _popupService;

    [ObservableProperty] private StartupShort _currentStartup;

    [ObservableProperty] private bool _isRefreshing;

    private bool _showRefreshMessageEnd = false;
    private bool _showRefreshMessageStart = false;

    private readonly List<int> _startupdIds = new();

    [ObservableProperty] private ObservableCollection<StartupShort> _startups = new();


    public StartupsViewModel(IPopupService popupService, ApiServiceProvider apiServiceProvider)
    {
        _popupService = popupService;
        _apiServiceProvider = apiServiceProvider;

        Startups.CollectionChanged += (sender, args) => OnPropertyChanged(nameof(Startups));

        WeakReferenceMessenger.Default.Register<StartupDonatedChangedMessage>(this, async (r, m) =>
        {
            var startup = Startups.FirstOrDefault(s => s.Id == m.Value);
            if (startup != null)
            {
                var response = await _apiServiceProvider.CallWebApi<int>($"/api/Donated?id={startup.Id}");
                if (response.IsSuccess) startup.Donated = response.Data;
            }
        });
    }

    public void Dispose()
    {
        WeakReferenceMessenger.Default.Unregister<StartupDonatedChangedMessage>(this);
    }

    [RelayCommand]
    private async Task GetStartups()
    {
        _startupdIds.Clear();
        Startups.Clear();

        var response =
            await _apiServiceProvider.CallWebApi<List<int>, List<int>>("/api/StartupsIds", new List<int>(),
                method: HttpMethod.Get);
        if (response.IsSuccess)
        {
            _startupdIds.AddRange(response.Data);
            if (response.Data.Count > 0)
            {
                var ids = response.Data;
                foreach (var id in ids)
                {
                    var startup = await _apiServiceProvider.CallWebApi<StartupShort>($"/api/Startups?startupId={id}");
                    if (startup.IsSuccess)
                    {
                        var donatedResponse =
                            await _apiServiceProvider.CallWebApi<int>($"/api/Donated?id={startup.Data.Id}");
                        if (donatedResponse.IsSuccess)
                            startup.Data.Donated = donatedResponse.Data;
                        else
                            startup.Data.Donated = 0;

                        Startups.Add(startup.Data);
                    }
                    else
                    {
                        await ToastHelper.ShowToastAsync("Error when loading startups");
                    }
                }
            }
        }

        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task UpdateStartups()
    {
        var response =
            await _apiServiceProvider.CallWebApi<List<int>, List<int>>("/api/StartupsIds", _startupdIds,
                method: HttpMethod.Get);
        if (response.IsSuccess)
        {
            _startupdIds.AddRange(response.Data);
            if (response.Data.Count > 0)
            {
                var ids = response.Data;
                foreach (var id in ids)
                {
                    var startup = await _apiServiceProvider.CallWebApi<StartupShort>($"/api/Startups?startupId={id}");
                    if (startup.IsSuccess) Startups.Add(startup.Data);
                }
            }
        }
        else
        {
            await ToastHelper.ShowToastAsync("Error when loading startups");
        }

        IsRefreshing = false;
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
    private async Task Donate(StartupShort startup)
    {
        await _popupService.ShowPopupAsync<DonationViewModel>(viewModel =>
            viewModel.StartupId = startup.Id);
    }
}