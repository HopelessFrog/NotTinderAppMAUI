using NotTinderAppMAUI.ViewModels;

namespace NotTinderAppMAUI.Pages;

public partial class StartupsPage : ContentPage, IDisposable
{
    public StartupsPage(StartupsViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }

    public void Dispose()
    {
        (BindingContext as IDisposable)?.Dispose();
    }
}