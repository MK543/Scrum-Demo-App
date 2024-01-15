using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using NavigationTutorial.Core;
using NavigationTutorial.Services;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.ViewModel;

public class MembersViewModel : Core.ViewModel
{
    public List<User> ProjectMembers { get; set; }
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
    
    public ICommand NavigateToManageUsersView { get; set; }
    
    private User _currentUser;

    public User CurrentUser
    {
        get => _currentUser;
        set
        {
            _currentUser = value;
            OnPropertyChanged();
        }
    }
    
    public MembersViewModel(INavigationService navigationService)
    {
        Console.Out.WriteLine("MembersViewModel Created");
        CurrentUser = ((App)Application.Current).LoggedUser;
        Console.Out.WriteLine(CurrentUser);
        Navigation = navigationService;
        NavigateToManageUsersView = new RelayCommands(o => {Navigation.NavigateTo<ManageUsersViewModel>();}, o => true);
        LoadMembers();
        var app = (App)Application.Current;
        app.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(app.ProjectId))
            {
                LoadMembers();
            }
        };
    }

    public void LoadMembers()
    {
        var userProjects = ((App)Application.Current).LoggedUser.Projects;
        if (userProjects != null && userProjects.Any())
        {
            var  project = userProjects.FirstOrDefault(p => p.Id == ((App)Application.Current).ProjectId);
            ProjectMembers = project.Users;
        }
        else
        {
            ProjectMembers = null;
        }
    }
    
}