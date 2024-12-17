namespace NotTinderAppMAUI.Models.Authenticate;

public class AuthenticateRequest
{
    public string DeviceId { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public bool Remember { get; set; }
}