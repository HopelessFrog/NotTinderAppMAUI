namespace NotTinderAppMAUI.Models.DTOs;

public class LoginData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<string> Roles { get; set; } = new();
    public TokenDto Tokens { get; set; }
}