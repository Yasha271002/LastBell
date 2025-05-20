using LastBell.Models;
using LastBell.ViewModels;
using LastBell.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Services.ServiceCollectionExtensions;
using MvvmNavigationLib.Stores;
using Serilog;

namespace LastBell.HostBuilders
{
    public static class BuildMainNavigationExtension
    {
        public static IHostBuilder BuildMainNavigation(this IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<NavigationStore>();
                services.AddUtilityNavigationServices<NavigationStore>();
                services.AddNavigationService<MainPageViewModel, NavigationStore>();
                services.AddNavigationService<QuizPageViewModel, NavigationStore>();
                services.AddParameterNavigationService<ResultPageViewModel, NavigationStore, ResultModel>(s => param =>
                    new ResultPageViewModel(param,
                        s.GetRequiredService<NavigationService<MainPageViewModel>>(),
                        s.GetRequiredService<ILogger>()));
            });

            return builder;
        }
    }
}