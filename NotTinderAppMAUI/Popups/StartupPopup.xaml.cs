using CommunityToolkit.Maui.Views;
using NotTinderAppMAUI.ViewModels;

namespace NotTinderAppMAUI.Popups;

public partial class StartupPopup : Popup
{
    public StartupPopup(StartupViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}