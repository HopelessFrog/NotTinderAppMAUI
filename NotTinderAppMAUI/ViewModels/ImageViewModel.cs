using CommunityToolkit.Mvvm.ComponentModel;

namespace NotTinderAppMAUI.ViewModels;

public partial class ImageViewModel : ObservableObject
{
    [ObservableProperty] private string image;
}