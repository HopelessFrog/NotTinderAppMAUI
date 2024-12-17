using CommunityToolkit.Mvvm.ComponentModel;

namespace NotTinderAppMAUI.Models.DTOs;

public partial class TopStartupDto : ObservableObject
{
    [ObservableProperty] private int _donated;
    public int Id { get; set; }
    public string Icon { get; set; }
    public string Name { get; set; }
}