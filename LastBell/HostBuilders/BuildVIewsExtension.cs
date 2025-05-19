using CommunityToolkit.Mvvm.Messaging;
using LastBell.Managers;
using LastBell.Models;
using LastBell.ViewModels;
using LastBell.ViewModels.Pages;
using LastBell.ViewModels.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;

namespace LastBell.HostBuilders
{
    public static class BuildViewsExtension
    {
        public static IHostBuilder BuildViews(this IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                var inactivityConfig = context.Configuration.GetSection("inactivity").Get<InactivityConfig>();
                services.AddSingleton<IMessenger>(_ => new WeakReferenceMessenger());
                services.AddSingleton<JsonManager>();
                services.AddSingleton<InactivityManager<MainPageViewModel>>(s => new InactivityManager<MainPageViewModel>(
                    inactivityConfig ?? new InactivityConfig(60, 10),
                    s.GetRequiredService<NavigationStore>(),
                    s.GetRequiredService<NavigationService<MainPageViewModel>>(),
                    s.GetRequiredService<CloseNavigationService<ModalNavigationStore>>()));

                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton(s => new Views.Windows.MainWindow()
                {
                    DataContext = s.GetRequiredService<MainWindowViewModel>()
                });
            });
            return builder;
        }
    }
}
