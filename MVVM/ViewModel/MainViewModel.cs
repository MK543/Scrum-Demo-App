using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using NavigationTutorial.Core;
using NavigationTutorial.Services;

namespace NavigationTutorial.MVVM.ViewModel;

public class MainViewModel : Core.ViewModel
 {
   private INavigationService _navigation;

   public INavigationService Navigation
    {
     get => _navigation;
     set
     {
      _navigation = value;
      OnPropertyChanged();
     }
    }

   public ICommand NavigateToHomeView { get; set; }
   public ICommand NavigateToRegisterView { get; set; }
   public ICommand NavigateToAboutUsView { get; set; }
   
   public ICommand ExitAppCommand { get; set; }
   
   public ICommand OpenFacebookCommand { get; }
   public ICommand OpenInstagramCommand { get; }
   public ICommand OpenTwitterCommand { get; }
   public ICommand OpenYoutubeCommand { get; }

  
  
   public MainViewModel(INavigationService navigationService)
     {
        Navigation = navigationService;
        NavigateToHomeView = new RelayCommands(o => {Navigation.NavigateTo<HomeViewModel>();}, o => true);
        NavigateToRegisterView = new RelayCommands(o => {Navigation.NavigateTo<RegisterViewModel>();}, o => true);
        NavigateToAboutUsView = new RelayCommands(o => {Navigation.NavigateTo<AboutUsViewModel>();}, o => true);
        ExitAppCommand = new RelayCommand(ExitApp);
        OpenFacebookCommand = new RelayCommand(() =>  Process.Start(new ProcessStartInfo
        {
         FileName = "https://www.facebook.com/",
         UseShellExecute = true
        }));
        OpenInstagramCommand = new RelayCommand(() =>  Process.Start(new ProcessStartInfo
        {
         FileName = "https://www.instagram.com/",
         UseShellExecute = true
        }));
        OpenTwitterCommand = new RelayCommand(() =>  Process.Start(new ProcessStartInfo
        {
         FileName = "https://twitter.com/",
         UseShellExecute = true
        }));
        OpenYoutubeCommand = new RelayCommand(() =>  Process.Start(new ProcessStartInfo
        {
         FileName = "https://www.youtube.com/",
         UseShellExecute = true
        }));
     }

     public void ExitApp()
     {
      MessageBoxResult result = MessageBox.Show("Do you want to log out?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
      if (result == MessageBoxResult.Yes)  ((App)Application.Current).ExitApp();
     }
   
   
 }