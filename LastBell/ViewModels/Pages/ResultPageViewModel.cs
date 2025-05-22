using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LastBell.Helpers;
using LastBell.Helpers.Messages;
using LastBell.Models;
using MvvmNavigationLib.Services;
using Serilog;

namespace LastBell.ViewModels.Pages;

public partial class ResultPageViewModel(ResultModel result, NavigationService<MainPageViewModel> mainPageNavigationService, ILogger logger, IMessenger messenger) : ObservableObject
{
    [ObservableProperty] private ResultModel _result = result;
    [ObservableProperty] private string _image = string.Empty;
    private readonly PathHelper pathHelper = new();

    [RelayCommand] private void MainPageNavigation() => mainPageNavigationService.Navigate();

    [RelayCommand]
    private void Loaded()
    {
        Image = pathHelper.ResolveImagePath(Result.ImagePath, "Resources\\ResultImages", logger);
        Result.Profession = Result.Profession.ToUpper();
        messenger.Send(new VideoSelectionMessage(Result.Category));
    }
}