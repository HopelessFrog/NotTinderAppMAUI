using System.Reflection;
using NotTinderAppMAUI.Models.User;
using NotTinderAppMAUI.Pages;
using NotTinderAppMAUI.ViewModels;

namespace NotTinderAppMAUI;

public partial class AppShell : Shell
{
    public static readonly string UserId;
    private readonly HashSet<object> _disposedPages = new();
    private readonly IServiceProvider _serviceProvider;
    private readonly UserManager _userManager;
    private ShellContent _previousShellContent;


    public AppShell(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _userManager = serviceProvider.GetService<UserManager>();
        BindingContext = serviceProvider.GetService<HeaderViewModel>();
        Appearing += AppShell_Appearing;


        Routing.RegisterRoute(nameof(LoginPage) + "/" + nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute("LoginPage", typeof(LoginPage));
        Routing.RegisterRoute("MainPage", typeof(TabBar));

        InitializeComponent();
    }

    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);
        if (CurrentItem?.CurrentItem?.CurrentItem is not null &&
            _previousShellContent is not null)
        {
            var property = typeof(ShellContent)
                .GetProperty("ContentCache",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                    BindingFlags.FlattenHierarchy);
            property.SetValue(_previousShellContent, null);
            (BindingContext as HeaderViewModel).Dispose();
            BindingContext = _serviceProvider.GetService<HeaderViewModel>();
        }

        _previousShellContent = CurrentItem?.CurrentItem?.CurrentItem;
    }


    private async void AppShell_Appearing(object? sender, EventArgs e)
    {
        await GoToAsync("//LoginPage");
    }
}