using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using NotTinderAppMAUI.Helprers;
using NotTinderAppMAUI.Models;
using NotTinderAppMAUI.Models.User;
using NotTinderAppMAUI.Pages;
using NotTinderAppMAUI.Popups;
using NotTinderAppMAUI.Services;
using NotTinderAppMAUI.Services.Interfaces;
using NotTinderAppMAUI.ViewModels;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace NotTinderAppMAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseSkiaSharp()
            .ConfigureMauiHandlers(handlers =>
            {
#if ANDROID
                    handlers.AddHandler(typeof(Shell),typeof(CustomShellRenderer));
#endif
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Verdana.ttf", "OpenSansRegular");
                fonts.AddFont("Verdana.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<HttpsConnectionHelper>();
        builder.Services.AddSingleton<ApiServiceProvider>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<UserManager>();
        builder.Services.AddSingleton<IBalanceService, BalanceService>();
        builder.Services.AddSingleton<RegisterViewModel>();
        builder.Services.AddSingleton<RegisterPage>();
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<HeaderViewModel>();
        builder.Services.AddTransient<StartupsPage>();
        builder.Services.AddTransient<StartupsViewModel>();
        builder.Services.AddTransient<MyStartupsPage>();
        builder.Services.AddTransient<MyStarupsViewModel>();
        builder.Services.AddTransient<TopStartupsViewModel>();
        builder.Services.AddTransient<TopStartupsPage>();
        builder.Services.AddTransient<TopsDonatersPage>();
        builder.Services.AddTransient<TopDonatersViewModel>();
        builder.Services.AddTransient<AdminPage>();
        builder.Services.AddTransient<AdminViewModel>();
        builder.Services.AddTransientPopup<PutStartupPopup, AddStartupViewModel>();
        builder.Services.AddTransientPopup<UpdateStartupPopup, UpdateStartupViewModel>();
        builder.Services.AddTransientPopup<TopAppBalancePopup, TopUpBalanceViewModel>();
        builder.Services.AddTransientPopup<StartupPopup, StartupViewModel>();
        builder.Services.AddTransientPopup<ImagePopup, ImageViewModel>();
        builder.Services.AddTransientPopup<DonationPopup, DonationViewModel>();
        builder.Services.AddTransientPopup<TransactionsStatPopup, TransactionsViewModel>();


#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}