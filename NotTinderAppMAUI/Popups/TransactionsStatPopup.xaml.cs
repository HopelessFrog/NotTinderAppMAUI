using CommunityToolkit.Maui.Views;
using NotTinderAppMAUI.ViewModels;

namespace NotTinderAppMAUI.Popups;

public partial class TransactionsStatPopup : Popup
{
    public TransactionsStatPopup(TransactionsViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}