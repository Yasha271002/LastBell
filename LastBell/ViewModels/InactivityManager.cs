using CommunityToolkit.Mvvm.ComponentModel;
using LastBell.Helpers;
using LastBell.Models;
using MvvmNavigationLib.Services;
using MvvmNavigationLib.Stores;
using System.Diagnostics;

namespace LastBell.ViewModels
{
    public class InactivityManager<TInitialViewModel> : IDisposable
    where TInitialViewModel : ObservableObject
    {
        private readonly NavigationStore _mainStore;
        private readonly INavigationService _initialNavigationService;
        private readonly INavigationService _closePopupNavigationService;
        private readonly BaseInactivityHelper _inactivity;
        private readonly BaseInactivityHelper _passwordInactivity;

        private InactivityConfig Config { get; }

        public InactivityManager(
            InactivityConfig config,
            NavigationStore mainStore,
            INavigationService initialNavigationService,
            INavigationService closePopupNavigationService)
        {
            _mainStore = mainStore;
            _initialNavigationService = initialNavigationService;
            _closePopupNavigationService = closePopupNavigationService;
            Config = config;
            _inactivity = new BaseInactivityHelper(Config.InactivityTime);
            _passwordInactivity = new BaseInactivityHelper(Config.PasswordInactivityTime);
        }

        public void Activate()
        {
            _inactivity.OnInactivity += _inactivity_OnInactivity;
            _passwordInactivity.OnInactivity += _passwordInactivity_OnInactivity;
        }

        public void Dispose()
        {
            _inactivity.OnInactivity -= _inactivity_OnInactivity;
            _passwordInactivity.OnInactivity -= _passwordInactivity_OnInactivity;
        }

        private void _passwordInactivity_OnInactivity(int inactivityTime) => _closePopupNavigationService.Navigate();

        private void _inactivity_OnInactivity(int inactivityTime)
        {
            try
            {
                var processes = Process.GetProcessesByName("ChromeHelper");
                var processes2 = Process.GetProcesses()
                    .Where(p => p.ProcessName.ToLower().Contains("chrome"))
                    .ToArray();


                foreach (var process in processes)
                {
                    if (!process.HasExited)
                    {
                        process.CloseMainWindow();
                    }
                }
                foreach (var process in processes2)
                {
                    if (!process.HasExited)
                    {
                        process.CloseMainWindow();
                    }
                }
            }
            catch (Exception ex)
            {
               
            }


            if (_mainStore.CurrentViewModel is TInitialViewModel) return;
            _initialNavigationService.Navigate();
        }
    }
}