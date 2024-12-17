using CommunityToolkit.Mvvm.ComponentModel;

namespace NotTinderAppMAUI.Models;

public partial class StartupShort : ObservableObject
{
    [ObservableProperty] private int _donated;

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Images { get; set; }
    public string OwnerId { get; set; }
    public int DonationTarget { get; set; }
    public string Icon { get; set; }
}