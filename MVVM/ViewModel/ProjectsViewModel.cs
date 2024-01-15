using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using NavigationTutorial.Core;
using NavigationTutorial.Services;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.ViewModel;

public class ProjectsViewModel : Core.ViewModel
{


    public string SelectedProjectName { get; set; }
    
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
    
    public ICommand NavigateToManageProjectsView { get; set; }
    public List<Project> UserProjects { get; set; }
    
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
    
    public ProjectsViewModel(INavigationService navigationService)
    {
        CurrentUser = ((App)Application.Current).LoggedUser;
        Console.Out.WriteLine(CurrentUser);
        Navigation = navigationService;
        NavigateToManageProjectsView = new RelayCommands(o => {Navigation.NavigateTo<ManageProjectsViewModel>();}, o => true);
        UserProjects = ((App)Application.Current).LoggedUser.Projects;
        
        foreach (var project in UserProjects)
        {
            var projectManager = project.Users.FirstOrDefault(u => u.Id == project.ProjectCreatorId);
            project.ProjectManager = projectManager;
        }
        
        if (UserProjects != null && UserProjects.Any())
        {
            SelectedProjectName = UserProjects.FirstOrDefault(project => project.Id == ((App)Application.Current).ProjectId)?.ProjectName;
        }
        
        else
        {
            SelectedProjectName = null;
        }
    }
}