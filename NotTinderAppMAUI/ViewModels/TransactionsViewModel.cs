using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotTinderAppMAUI.Models;
using NotTinderAppMAUI.Models.DTOs;

namespace NotTinderAppMAUI.ViewModels;

public partial class TransactionsViewModel : ObservableObject
{
    private readonly ApiServiceProvider _apiServiceProvider;
    private readonly IPopupService _popupService;

    [ObservableProperty] private bool _isRefreshing;

    [ObservableProperty] private ObservableCollection<TransactionDto> _transactions = new();

    public TransactionsViewModel(ApiServiceProvider apiServiceProvider, IPopupService popupService)
    {
        _apiServiceProvider = apiServiceProvider;
        _popupService = popupService;


        Transactions.CollectionChanged += (sender, args) => OnPropertyChanged(nameof(IsTransactionNot));
    }

    public Guid UserId { get; set; }

    public bool IsTransactionNot => Transactions.Count == 0;

    [RelayCommand]
    public async Task LoadTransactions()
    {
        var response =
            await _apiServiceProvider.CallWebApi<List<TransactionDto>>($"/api/UserTransactions?id={UserId}",
                method: HttpMethod.Get);

        if (response.IsSuccess)
        {
            Transactions.Clear();
            if (response?.Data == null)
            {
                IsRefreshing = false;
                return;
            }

            foreach (var transaction in response.Data) Transactions.Add(transaction);
        }
        else
        {
            await Task.Delay(500);
        }

        IsRefreshing = false;
    }
}