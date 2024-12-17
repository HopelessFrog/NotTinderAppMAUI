using NotTinderAppMAUI.ViewModels;

namespace NotTinderAppMAUI.Pages;

public partial class TopsDonatersPage : ContentPage
{
    public TopsDonatersPage(TopDonatersViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}