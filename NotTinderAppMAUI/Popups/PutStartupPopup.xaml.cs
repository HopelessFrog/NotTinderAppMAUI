using CommunityToolkit.Maui.Views;
using NotTinderAppMAUI.ViewModels;

namespace NotTinderAppMAUI.Popups;

public partial class PutStartupPopup : Popup
{
    public PutStartupPopup(AddStartupViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}