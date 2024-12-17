using NotTinderAppMAUI.ViewModels;

namespace NotTinderAppMAUI.Pages;

public partial class AdminPage : ContentPage
{
    public AdminPage(AdminViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}