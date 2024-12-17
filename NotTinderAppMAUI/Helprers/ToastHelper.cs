using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace NotTinderAppMAUI.Helprers;

public static class ToastHelper
{
    public static async Task ShowToastAsync(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            message = "Iternal error, try again later.";
        var toast = Toast.Make(message, ToastDuration.Short, 12);
        await toast.Show();
    }
}