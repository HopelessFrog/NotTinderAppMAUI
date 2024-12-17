using CommunityToolkit.Maui.Views;
using NotTinderAppMAUI.ViewModels;

namespace NotTinderAppMAUI.Popups;

public partial class DonationPopup : Popup
{
    public DonationPopup(DonationViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}