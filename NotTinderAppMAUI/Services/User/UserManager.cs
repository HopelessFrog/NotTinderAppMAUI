namespace NotTinderAppMAUI.Models.User;

public class UserManager
{
    private readonly ApiServiceProvider _apiServiceProvider;


    public UserManager(ApiServiceProvider apiServiceProvider)
    {
        _apiServiceProvider = apiServiceProvider;
    }


    public async Task LogOut()
    {
        await _apiServiceProvider.CallWebApi("/api/LogOut", method: HttpMethod.Post);
        Preferences.Clear();
        SecureStorage.RemoveAll();
    }
}