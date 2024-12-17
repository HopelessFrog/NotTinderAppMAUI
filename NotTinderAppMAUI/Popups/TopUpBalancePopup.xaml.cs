using CommunityToolkit.Maui.Views;
using NotTinderAppMAUI.ViewModels;

namespace NotTinderAppMAUI.Popups;

public partial class TopAppBalancePopup : Popup
{
    public TopAppBalancePopup(TopUpBalanceViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void InputView_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var entry = (Entry)sender;

        GC.SuppressFinalize(entry);

        if (!string.IsNullOrEmpty(entry.Text) && !entry.Text.All(char.IsDigit)) entry.Text = e.OldTextValue;
    }
}