using CommunityToolkit.Mvvm.Messaging;
using NotTinderAppMAUI.Messages;
using NotTinderAppMAUI.Models;
using NotTinderAppMAUI.Services.Interfaces;

namespace NotTinderAppMAUI.Services;

public class BalanceService : IBalanceService
{
    private readonly ApiServiceProvider _apiServiceProvider;

    public BalanceService(ApiServiceProvider apiServiceProvider)
    {
        _apiServiceProvider = apiServiceProvider;
    }

    public async Task UpdateBalance()
    {
        var response =
            await _apiServiceProvider.CallWebApi<int>("/api/Balance",
                method: HttpMethod.Get);

        if (response.IsSuccess) WeakReferenceMessenger.Default.Send(new BalanceChangedMessage(response.Data));
    }
}