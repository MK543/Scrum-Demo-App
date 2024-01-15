using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using NavigationTutorial.Core;
using NavigationTutorial.MVVM.ViewModel.ManageProjectsOperationClass;
using NavigationTutorial.MVVM.ViewModel.ManageTasksOperationClass;
using NavigationTutorial.Services;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.ViewModel;

public class ManageProjectsViewModel : Core.ViewModel
{
    private INavigationService _navigation;

    private readonly ScrumDbContext _scrumDbContext;
    
    public ScrumDbContext ScrumDbContext
    {
        get => _scrumDbContext;
    }

    public INavigationService Navigation
    {
        get => _navigation;
        set
        {
            _navigation = value;
            OnPropertyChanged();
        }
    }

    private string _selectedProjectName;

    public string SelectedProjectName
    {
        get { return _selectedProjectName; }
        set
        {
            if (_selectedProjectName != value)
            {
                _selectedProjectName = value;
                OnPropertyChanged(nameof(SelectedProjectName));
                Console.Out.WriteLine("Working?");
            }
        }
    }
    public List<Project> UserProjects { get; set; }
    public ICommand NavigateToProjectView { get; set; }
    public ICommand AddProjectCommand { get; }
    public ICommand UpdateProjectCommand { get;  }
    
    public ICommand RemoveProjectCommand { get; }
    
    public CreateProject CreateProject { get; set; }

    public DeleteProject DeleteProject { get; set; }
    public UpdateProject UpdateProject { get; set; }
    
    public ICommand NavigateToManageProjectsView { get; set; }
    
    public event EventHandler SelectedProjectChanged;

    public ManageProjectsViewModel(INavigationService navigationService, CreateProject cp, DeleteProject dp, UpdateProject up)
    {
        Navigation = navigationService;
        NavigateToProjectView = new RelayCommands(o => {Navigation.NavigateTo<ProjectsViewModel>();}, o => true);
        NavigateToManageProjectsView = new RelayCommands(o => {Navigation.NavigateTo<ManageProjectsViewModel>();}, o => true);
        AddProjectCommand = new RelayCommand(AddProject);
        UpdateProjectCommand = new RelayCommand(UpdateExistingProject);
        RemoveProjectCommand = new RelayCommand(RemoveProject);
        CreateProject = cp;
        DeleteProject = dp;
        UpdateProject = up;
        UpdateProject.PropertyChanged += UpdateProject_PropertyChanged;
        
        var loggedUserId = ((App)Application.Current).LoggedUser.Id;
        UserProjects = ((App)Application.Current).LoggedUser.Projects.Where(project => project.ProjectCreatorId == loggedUserId).ToList();

        Console.Out.WriteLine("Manage Tasks View Class Created");
        // LoadMembers();
        // LoadTasks();
        _scrumDbContext = new ScrumDbContext();
        
        // drag and drop list users load
        LoadUsers();

    }
    
    private async void AddProject()
    {
        Project newProject = CreateProject.CreateNewProject();
        if (newProject != null)
        {
            var userId = ((App)Application.Current).LoggedUser.Id;
            var loggedUserApp = _scrumDbContext.Users.FirstOrDefault(u => u.Id == userId);

            _scrumDbContext.Projects.Add(newProject);
            _scrumDbContext.SaveChanges();
            foreach (var user in InProjectUserListingViewModel.UserViewModels)
            {
                var userApp = _scrumDbContext.Users.FirstOrDefault(u => u.Id == user.Id);
                userApp.Projects.Add(newProject);
                _scrumDbContext.SaveChanges();
            }
            
            loggedUserApp.Projects.Add(newProject);
            _scrumDbContext.SaveChanges();
            
            
            //TODO Refresh User Task
            //await ((App)Application.Current).LoadUserData(((App)Application.Current).LoggedUser.Id);
            await ((App)Application.Current).UpdateUserData(((App)Application.Current).LoggedUser.Id);
            LoadUsers();
            Console.Out.WriteLine("Project added successfully!");
            NavigateToManageProjectsView.Execute(true);
        }
    }
    
    private async void UpdateExistingProject()
    {
        Console.Out.WriteLine("Update Project");
        System.Diagnostics.Debug.WriteLine("Update Project");
        Project updatedProject = UpdateProject.UpdateExistingProject();

        
        
        if (updatedProject != null)
        {
            var oldProject = _scrumDbContext.Projects
                .Include(p => p.ProjectTasks)
                .Include(p => p.Users)
                .FirstOrDefault(t => t.Id == updatedProject.Id);

            var loggedUserId = ((App)Application.Current).LoggedUser.Id;
            
            if (oldProject != null)
            {
                var oldProjectName = oldProject.ProjectName;
                oldProject.ProjectName = updatedProject.ProjectName;
                updatedProject.ProjectName = oldProjectName;
                oldProject.ProjectDescription = updatedProject.ProjectDescription;
                oldProject.FinishDate = updatedProject.FinishDate;
                for (int i = oldProject.Users.Count - 1; i >= 0; i--)
                {
                    var user = oldProject.Users[i];
                    System.Diagnostics.Debug.WriteLine(user.FirstName);
                    if (!InProjectUserListingViewModel.UserViewModels.Any(u => u.Id == user.Id) && user.Id != loggedUserId)
                    {
                        foreach (var task in oldProject.ProjectTasks.Where(t => t.AssignedUserId == user.Id))
                        {
                            task.AssignedUserId = oldProject.ProjectCreatorId;
                        }
                        oldProject.Users.Remove(user);
                    }
                }
                
                foreach (var user in InProjectUserListingViewModel.UserViewModels)
                {
                    if (!oldProject.Users.Any(u => u.Id == user.Id))
                    {
                        var userToAdd = _scrumDbContext.Users.FirstOrDefault(u => u.Id == user.Id);
                        if (userToAdd != null)
                        {
                            oldProject.Users.Add(userToAdd);
                        }
                    }
                }

                _scrumDbContext.Projects.Update(oldProject);
                _scrumDbContext.SaveChanges();

                //TODO Refresh User Task
                //await ((App)Application.Current).LoadUserData(((App)Application.Current).LoggedUser.Id);
                await ((App)Application.Current).UpdateUserData(((App)Application.Current).LoggedUser.Id);
                LoadUsers();
                Console.Out.WriteLine("Project updated successfully!");
                NavigateToManageProjectsView.Execute(true);
                
            }
            else
            {
                Console.Out.WriteLine("Project not found!");
            }
        }
    }
    private void UpdateProject_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(UpdateProject.SelectedProject))
        {
            //System.Diagnostics.Debug.WriteLine("Zmieniono projekt do update, zaladuj liste uzytkownikow od nowa");
            LoadExistingProjectUsers();
        }
    }

    
    private async void RemoveProject()
    {
        Console.Out.WriteLine("Remove Project");
        Project removeProject = DeleteProject.RemoveProject();
        if (removeProject != null)
        {
            Project temp = _scrumDbContext.Projects.FirstOrDefault(t => t.Id == removeProject.Id);
            if (temp != null)
            {
                _scrumDbContext.Projects.Remove(temp);
                _scrumDbContext.SaveChanges();

                //TODO Refresh User Task
                await ((App)Application.Current).LoadUserData(((App)Application.Current).LoggedUser.Id);
                Console.Out.WriteLine("Project deleted successfully!");
                NavigateToManageProjectsView.Execute(true);
            }
            else
            {
                Console.Out.WriteLine("Project not found!");
            }
        }
    }
    
    
    // dodawanie list drag and drop
    
    private UserListingViewModel _inProjectUserListingViewModel;
    private UserListingViewModel _notInProjectUserListingViewModel;

    public UserListingViewModel InProjectUserListingViewModel
        {
            get => _inProjectUserListingViewModel;
            set
            {
                _inProjectUserListingViewModel = value;
                OnPropertyChanged(nameof(InProjectUserListingViewModel));
            }
        }

    public UserListingViewModel NotInProjectUserListingViewModel
    {
        get => _notInProjectUserListingViewModel;
        set
        {
            _notInProjectUserListingViewModel = value;
            OnPropertyChanged(nameof(NotInProjectUserListingViewModel));
        }
    }

    public void LoadUsers()
    {
        UserListingViewModel listInProject = new UserListingViewModel();
        UserListingViewModel listNotInProject = new UserListingViewModel();

        using (var dbContext = new ScrumDbContext())
        {
            var users = dbContext.Users.ToList();
            foreach (var user in users)
            {
                if (user.Id != ((App)Application.Current).LoggedUser.Id)
                {
                    listNotInProject.AddUser(user);
                }
            }
        }

        ChangeInProjectList(listInProject);
        ChangeNotInProjectList(listNotInProject);
    }

    public void LoadExistingProjectUsers()
    {
        UserListingViewModel listInProject = new UserListingViewModel();
        UserListingViewModel listNotInProject = new UserListingViewModel();

        if (UpdateProject.SelectedProject != null)
        {
            using (var dbContext = new ScrumDbContext())
            {
                var selectedProjectId = UpdateProject.SelectedProject.Id;
                var usersNotInProject = dbContext.Users
                    .Where(u => !u.Projects.Any(p => p.Id == selectedProjectId))
                    .ToList();
                var usersInProject = dbContext.Users
                    .Where(u => u.Projects.Any(p => p.Id == selectedProjectId))
                    .ToList();
            
                foreach (var user in usersInProject)
                {
                    if (user.Id != ((App)Application.Current).LoggedUser.Id)
                    {
                        listInProject.AddUser(user);
                    }
                }
                foreach (var user in usersNotInProject)
                {
                    listNotInProject.AddUser(user);
                }
            }

            ChangeInProjectList(listInProject);
            ChangeNotInProjectList(listNotInProject);
        }
    }

    public void ChangeInProjectList(UserListingViewModel newList)
    {
        InProjectUserListingViewModel = newList;
    }

    public void ChangeNotInProjectList(UserListingViewModel newList)
    {
        NotInProjectUserListingViewModel = newList;
    }
}