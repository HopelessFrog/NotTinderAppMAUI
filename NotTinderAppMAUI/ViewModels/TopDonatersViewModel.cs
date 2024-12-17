using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotTinderAppMAUI.Models;
using NotTinderAppMAUI.Models.DTOs;

namespace NotTinderAppMAUI.ViewModels;

public partial class TopDonatersViewModel : ObservableObject
{
    private readonly ApiServiceProvider _apiServiceProvider;

    [ObservableProperty] private ObservableCollection<DonaterDto> _donaters = new();
    [ObservableProperty] private bool _isRefreshing;

    public TopDonatersViewModel(IPopupService popupService, ApiServiceProvider apiServiceProvider)
    {
        _apiServiceProvider = apiServiceProvider;


        Task.Run(async () => await LoadDonaters());
    }

    [RelayCommand]
    private async Task LoadDonaters()
    {
        var donaterResponse = await _apiServiceProvider.CallWebApi<List<DonaterDto>>("/api/TopDonaters");
        if (donaterResponse.IsSuccess)
        {
            Donaters.Clear();
            if (donaterResponse?.Data == null)
            {
                IsRefreshing = false;
                return;
            }

            foreach (var donater in donaterResponse.Data) Donaters.Add(donater);
        }
        else
        {
            await Task.Delay(500);
        }

        IsRefreshing = false;
    }
}