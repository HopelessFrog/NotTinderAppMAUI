using CommunityToolkit.Maui.Views;
using NotTinderAppMAUI.ViewModels;

namespace NotTinderAppMAUI.Popups;

public partial class UpdateStartupPopup : Popup
{
    public UpdateStartupPopup(UpdateStartupViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}