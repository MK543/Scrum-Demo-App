using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using NavigationTutorial.Core;
using NavigationTutorial.Services;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.ViewModel;
public class ProjectsWindowViewModel : Core.ViewModel
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
    public ICommand NavigateToProjectsView { get; set; }
    public ICommand NavigateToTaskView { get; set; }
    public ICommand NavigateToMembersView { get; set; }
    
    public ICommand LogOutCommand { get; set; }
 
 
    public ProjectsWindowViewModel(INavigationService navigationService)
    {
        Navigation = navigationService;
        NavigateToProjectsView = new RelayCommands(o => {Navigation.NavigateTo<ProjectsViewModel>();}, o => true);
        NavigateToTaskView = new RelayCommands(o => {Navigation.NavigateTo<TaskViewModel>();}, o => true);
        NavigateToMembersView = new RelayCommands(o => {Navigation.NavigateTo<MembersViewModel>();}, o => true);
        LogOutCommand = new RelayCommand(LogOut);
    }

    public void LogOut()
    {
        MessageBoxResult result = MessageBox.Show("Do you want to log out?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)  ((App)Application.Current).ChangeToLoginWindow();
    }
}