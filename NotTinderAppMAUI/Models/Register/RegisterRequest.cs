using System.ComponentModel.DataAnnotations;

namespace NotTinderAppMAUI.Models.Register;

public class RegisterRequest
{
    public string Login { get; set; }
    public string Password { get; set; }

    [EmailAddress] public string Email { get; set; }
}