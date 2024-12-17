using NotTinderAppMAUI.ViewModels;

namespace NotTinderAppMAUI.Pages;

public partial class TopStartupsPage : ContentPage
{
    public TopStartupsPage(TopStartupsViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}