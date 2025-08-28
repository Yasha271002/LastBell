using CommunityToolkit.Mvvm.Messaging;
using LastBell.Models;
using LastBell.ViewModels.Pages;
using LastBell.ViewModels.Popups;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Services.ServiceCollectionExtensions;
using MvvmNavigationLib.Stores;
using Serilog;

namespace LastBell.HostBuilders
{
    public static class BuildModalNavigationExtension
    {
        public static IHostBuilder BuildModalNavigation(this IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<ModalNavigationStore>();
                services.AddUtilityNavigationServices<ModalNavigationStore>();
                //services.AddNavigationService<ExitPopupViewModel, ModalNavigationStore>();
                services.AddParameterNavigationService<ExitPopupViewModel, ModalNavigationStore, bool>(s => param =>
                    new ExitPopupViewModel(s.GetRequiredService<CloseNavigationService<ModalNavigationStore>>(),
                        s.GetRequiredService<NavigationService<MainPageViewModel>>(),
                        s.GetRequiredService<NavigationService<StartPageViewModel>>(),
                        param));
                services.AddNavigationService<PasswordPopupViewModel, ModalNavigationStore>(s => new PasswordPopupViewModel(
                    s.GetRequiredService<CloseNavigationService<ModalNavigationStore>>(),
                    context.Configuration.GetValue<string>("exitPassword") ?? "1234"));
            });

            return builder;
        }
    }
}
