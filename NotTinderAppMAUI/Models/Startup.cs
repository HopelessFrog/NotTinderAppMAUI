using NotTinderAppMAUI.Models.DTOs;

namespace NotTinderAppMAUI.Models;

public class Startup
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Images { get; set; }
    public string OwnerId { get; set; }
    public int DonationTarget { get; set; }
    public string Icon { get; set; }
    public int Donated { get; set; }
    public List<UserDto> Donaters { get; set; }
}