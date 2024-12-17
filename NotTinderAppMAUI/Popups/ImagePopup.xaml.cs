using CommunityToolkit.Maui.Views;
using NotTinderAppMAUI.ViewModels;

namespace NotTinderAppMAUI.Popups;

public partial class ImagePopup : Popup
{
    public ImagePopup(ImageViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}