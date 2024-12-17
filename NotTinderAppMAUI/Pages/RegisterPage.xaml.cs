using NotTinderAppMAUI.ViewModels;

namespace NotTinderAppMAUI.Pages;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}