using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmNavigationLib.Services;
using System.Diagnostics;
using System.IO;

namespace LastBell.ViewModels.Pages;

public partial class StartPageViewModel(NavigationService<MainPageViewModel> mainPageNavigationService) : ObservableObject
{
    [RelayCommand]
    private void GoMainView() => mainPageNavigationService.Navigate();

    [RelayCommand]
    private void LaunchHttpHelper()
    {
        try
        {
          
            string helperPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "HttpHelperFramework",
                "ChromeHelper",
                "ChromeHelper.exe"
            );

            if (File.Exists(helperPath))
            {
                Process.Start(new ProcessStartInfo
                {
                    Arguments = "--fullscreen --kiosk",
                    FileName = helperPath,
                    WorkingDirectory = Path.GetDirectoryName(helperPath)
                });
            }
            else
            {
                Debug.WriteLine($"Хелпер не найден по пути: {helperPath}");
            }
        }
        catch (Exception ex)
        {
            
        }
    }
}
