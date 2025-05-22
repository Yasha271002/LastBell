using System.IO;
using LastBell.Helpers;
using LastBell.HostBuilders;
using LastBell.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvvmNavigationLib.Services;
using Serilog;
using System.Windows;
using LastBell.Managers;
using LastBell.Models;
using LastBell.Views.Windows;
using Application = System.Windows.Application;

namespace LastBell;

public partial class App : Application
{
    private readonly IHost _appHost = CreateHostBuilder().Build();

    public App()
    {
        DispatcherUnhandledException += App_DispatcherUnhandledException;
    }
    private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        if (DebugHelper.IsRunningInDebugMode) throw e.Exception;
        var logger = _appHost.Services.GetRequiredService<ILogger>();
        logger.Error(e.Exception, "Неизвестная ошибка");
        e.Handled = true;
    }

    private static IHostBuilder CreateHostBuilder(string[]? args = null)
    {
        return Host.CreateDefaultBuilder(args)
            .BuildConfiguration()
            .BuildLogging()
            .BuildMainNavigation()
            .BuildModalNavigation()
            .BuildApi()
            .BuildViews();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        var jsonManager = new JsonManager();
        var settings = jsonManager.ReadJson<ScreenModel>(Path.Combine(Directory.GetCurrentDirectory(), "screen"));
        var initialNavigationService =
            _appHost.Services.GetRequiredService<NavigationService<MainPageViewModel>>();
        initialNavigationService.Navigate();
        var videoScreen = Screen.AllScreens[settings.VideoScreen];
        var mainScreen = Screen.AllScreens[settings.MainScreen];

        MainWindow = _appHost.Services.GetRequiredService<MainWindow>();
        MainWindow.Left = mainScreen.Bounds.Left;
        MainWindow.Top = mainScreen.Bounds.Top;
        MainWindow.Width = mainScreen.Bounds.Width;
        MainWindow.Height = mainScreen.Bounds.Height;

        var secondWind = _appHost.Services.GetRequiredService<VideoWindow>();
        secondWind.WindowStyle = WindowStyle.None;
        secondWind.Left = videoScreen.Bounds.Left - 10;
        secondWind.Top = videoScreen.Bounds.Top - 10;
        secondWind.Width = videoScreen.Bounds.Width + 50;
        secondWind.Height = videoScreen.Bounds.Height + 50;

        MainWindow.Show();
        secondWind.Owner = MainWindow;
        secondWind.Show();
        await _appHost.StartAsync();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _appHost.StopAsync();
        _appHost.Dispose();
        base.OnExit(e);
    }
}