using System.Linq;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using NavigationTutorial.Core;
using NavigationTutorial.MVVM.ViewModel.ManageUsersOperationClass;
using NavigationTutorial.Services;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.ViewModel;

public class ManageUsersViewModel : Core.ViewModel
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
    private readonly ScrumDbContext _scrumDbContext;
    
    public ScrumDbContext ScrumDbContext
    {
        get => _scrumDbContext;
    }
    
    public ICommand NavigateToUserView { get; set; }
    public ICommand AddUserCommand { get; }
    public ICommand UpdateUserCommand { get; }
    public ICommand DeleteUserCommand { get; }
    public CreateProjectUser CreateProjectUser { get; set; }
    public UpdateProjectUser UpdateProjectUser { get; set; }
    public DeleteProjectUser DeleteProjectUser { get; set; }
    
    public ICommand NavigateToManageUsersView { get; set; }

    public ManageUsersViewModel(INavigationService navigationService, CreateProjectUser cpu, UpdateProjectUser upu, DeleteProjectUser dpu)
    {
        Navigation = navigationService;
        NavigateToUserView = new RelayCommands(o => {Navigation.NavigateTo<MembersViewModel>();}, o => true);
        NavigateToManageUsersView = new RelayCommands(o => {Navigation.NavigateTo<ManageUsersViewModel>();}, o => true);
        AddUserCommand = new RelayCommand(AddUser);
        UpdateUserCommand = new RelayCommand(UpdateUser);
        DeleteUserCommand = new RelayCommand(RemoveUser);
        CreateProjectUser = cpu;
        UpdateProjectUser = upu;
        DeleteProjectUser = dpu;
        
        _scrumDbContext = new ScrumDbContext();
    }
    
    private async void AddUser()
    {
        User newUser = CreateProjectUser.CreateUser();
        if (newUser != null)
        {
            _scrumDbContext.Users.Add(newUser);
            _scrumDbContext.SaveChanges();

            //A nie UpdateUserData zamiast LoadUserData?
            await ((App)Application.Current).UpdateUserData(((App)Application.Current).LoggedUser.Id);
            NavigateToManageUsersView.Execute(null);
        }
    }
    
    private async void UpdateUser()
    {
        User updatedUser = UpdateProjectUser.UpdateUser();
        if (updatedUser != null)
        {
            var oldUser = _scrumDbContext.Users.FirstOrDefault(t => t.Id == updatedUser.Id);
        
            if (oldUser != null)
            {
                oldUser.FirstName = updatedUser.FirstName;
                oldUser.LastName = updatedUser.LastName;
                oldUser.Email = updatedUser.Email;
                oldUser.Password = updatedUser.Password;

                _scrumDbContext.Users.Update(oldUser);
                _scrumDbContext.SaveChanges();
                
                //A nie UpdateUserData zamiast LoadUserData?
                await ((App)Application.Current).UpdateUserData(((App)Application.Current).LoggedUser.Id);
                NavigateToManageUsersView.Execute(null);
            }
        }
    }
    
    private async void RemoveUser()
    {
        User removeUser = DeleteProjectUser.RemoveUser();
        if (removeUser != null)
        {
            User temp = _scrumDbContext.Users.FirstOrDefault(t => t.Id == removeUser.Id);
            if (temp != null)
            {
                ReassignTasks(temp);
                _scrumDbContext.Users.Remove(temp);
                _scrumDbContext.SaveChanges();

                //TODO Refresh User Task
                await ((App)Application.Current).UpdateUserData(((App)Application.Current).LoggedUser.Id);
                NavigateToManageUsersView.Execute(null);
                
            }
        }
    }
    
    private void ReassignTasks(User user)
    {
        using (var dbContext = new ScrumDbContext())
        {
            var userData = dbContext.Users
                .Where(u => u.Id == user.Id)
                .Include(u => u.Projects)
                .ThenInclude(p => p.ProjectTasks)
                .Include(u => u.Projects)
                .ThenInclude(p => p.Users)
                .FirstOrDefault();

            if (userData.Projects != null && userData.Projects.Any())
            {
                foreach (var project in userData.Projects)
                {
                    foreach (var task in project.ProjectTasks.Where(t => t.AssignedUserId == user.Id))
                    {
                        System.Diagnostics.Debug.WriteLine("Zmiana taska assigment");
                        task.AssignedUserId = project.ProjectCreatorId;
                    }
                }
            }
            dbContext.SaveChanges();
        }
    }
}