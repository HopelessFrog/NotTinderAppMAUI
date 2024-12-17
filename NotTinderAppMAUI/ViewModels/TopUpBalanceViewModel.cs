using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using NotTinderAppMAUI.Helprers;
using NotTinderAppMAUI.Messages;
using NotTinderAppMAUI.Models;

namespace NotTinderAppMAUI.ViewModels;

public partial class TopUpBalanceViewModel : ObservableObject
{
    private readonly ApiServiceProvider _apiServiceProvider;
    private readonly IPopupService _popupService;

    [ObservableProperty] private float _exchangeRate;

    [ObservableProperty] private ISeries[] _series;

    [ObservableProperty] private int balance;

    public TopUpBalanceViewModel(ApiServiceProvider apiServiceProvider, IPopupService popupService)
    {
        _apiServiceProvider = apiServiceProvider;
        _popupService = popupService;
        YAxes =
        [
            new Axis
            {
                Labeler = Labelers.Currency
            }
        ];
        XAxes = [new DateTimeAxis(TimeSpan.FromMinutes(1), date => date.ToString("G"))];

        Series =
        [
            new ColumnSeries<DateTimePoint>
            {
                Values = Points
            }
        ];
    }

    public ICartesianAxis[] XAxes { get; }
    public ICartesianAxis[] YAxes { get; }

    public ObservableCollection<DateTimePoint> Points { get; set; } = new();

    private async Task UpdateExchangeRate()
    {
        var coinRateResponse = await _apiServiceProvider.CallWebApi<CoinRate>("/api/ActualCoinRate");
        if (coinRateResponse.IsSuccess)
            if (Points.Last().Value != coinRateResponse.Data.Value)
            {
                Points.Add(
                    new DateTimePoint
                        { Value = coinRateResponse.Data.Value, DateTime = coinRateResponse.Data.DateTime });
                ExchangeRate = coinRateResponse.Data.Value;
            }
    }

    [RelayCommand]
    private async Task BuyCoins()
    {
        if (balance == 0)
            return;
        if (await Shell.Current.DisplayAlert("Pay?",
                $"Would you like to pay {balance * ExchangeRate} $ for {balance} coins", "Yes", "No"))
        {
            var response =
                await _apiServiceProvider.CallWebApi<int>($"/api/Balance/TopUp?value={Balance}",
                    method: HttpMethod.Post);
            if (response.IsSuccess)
            {
                WeakReferenceMessenger.Default.Send(new BalanceChangedMessage(response.Data));
                await _popupService.ClosePopupAsync();
            }
            else
            {
                await ToastHelper.ShowToastAsync(response.Message);
            }
        }
    }


    [RelayCommand]
    private async Task LoadPoints()
    {
        var coinRatesResponse = await _apiServiceProvider.CallWebApi<List<CoinRate>>("/api/CoinRates");
        if (coinRatesResponse.IsSuccess)
        {
            var coinRates = coinRatesResponse.Data;
            foreach (var coinRate in coinRates)
                Points.Add(new DateTimePoint { DateTime = coinRate.DateTime, Value = coinRate.Value });

            ExchangeRate = coinRates.Last().Value;
            Device.StartTimer(TimeSpan.FromMinutes(1), () =>
            {
                Task.Run(async () => await UpdateExchangeRate());
                return true;
            });
        }
    }
}