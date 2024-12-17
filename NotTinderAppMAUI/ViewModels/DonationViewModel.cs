using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NotTinderAppMAUI.Helprers;
using NotTinderAppMAUI.Messages;
using NotTinderAppMAUI.Models;
using NotTinderAppMAUI.Models.DTOs;
using NotTinderAppMAUI.Services.Interfaces;

namespace NotTinderAppMAUI.ViewModels;

public partial class DonationViewModel : ObservableObject
{
    private readonly ApiServiceProvider _apiServiceProvider;
    private readonly IBalanceService _balanceService;
    private readonly IPopupService _popupService;

    [ObservableProperty] private int _balance;

    [ObservableProperty] private int _donationAmount;

    public DonationViewModel(ApiServiceProvider apiServiceProvider, IPopupService popupService,
        IBalanceService balanceService)
    {
        _apiServiceProvider = apiServiceProvider;
        _popupService = popupService;
        _balanceService = balanceService;

        Balance = WeakReferenceMessenger.Default.Send<BalanceRequestMessage>();
    }

    public int StartupId { get; set; }

    [RelayCommand]
    private async Task Donation()
    {
        var response = await _apiServiceProvider.CallWebApi("/api/Donate",
            new DonateDto { Amount = DonationAmount, StartupId = StartupId });
        if (!response.IsSuccess) await ToastHelper.ShowToastAsync(response.Message);


        WeakReferenceMessenger.Default.Send(new StartupDonatedChangedMessage(StartupId));
        await _balanceService.UpdateBalance();
        await _popupService.ClosePopupAsync();
    }
}