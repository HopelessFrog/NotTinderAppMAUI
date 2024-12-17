using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotTinderAppMAUI.Helprers;
using NotTinderAppMAUI.Models;
using NotTinderAppMAUI.Models.DTOs;

namespace NotTinderAppMAUI.ViewModels;

public partial class StartupViewModel : ObservableObject
{
    private readonly ApiServiceProvider _apiServiceProvider;
    private readonly IPopupService _popupService;


    [ObservableProperty] private bool _canDonate;

    [ObservableProperty] private ObservableCollection<DonaterDto> _donaters = new();
    [ObservableProperty] private bool _isRefreshing;

    [ObservableProperty] private StartupShort _startup;

    public StartupViewModel(ApiServiceProvider apiServiceProvider, IPopupService popupService)
    {
        _apiServiceProvider = apiServiceProvider;
        _popupService = popupService;
    }

    [RelayCommand]
    public async Task LoadDonaters()
    {
        Donaters.Clear();
        var response =
            await _apiServiceProvider.CallWebApi<List<DonaterDto>>($"/api/Donaters?id={Startup.Id}",
                method: HttpMethod.Get);
        if (response.IsSuccess)
            foreach (var donater in response.Data)
                Donaters.Add(donater);
        else
            await ToastHelper.ShowToastAsync("Failed to load donaters");
        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task OpenImage(string imageSource)
    {
        await _popupService.ShowPopupAsync<ImageViewModel>(viewModel => viewModel.Image = imageSource);
    }

    [RelayCommand]
    private async Task Donate()
    {
        await _popupService.ShowPopupAsync<DonationViewModel>(viewModel =>
            viewModel.StartupId = Startup.Id);
    }
}