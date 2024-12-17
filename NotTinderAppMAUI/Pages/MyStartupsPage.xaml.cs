using NotTinderAppMAUI.ViewModels;

namespace NotTinderAppMAUI.Pages;

public partial class MyStartupsPage : ContentPage
{
    public MyStartupsPage(MyStarupsViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}