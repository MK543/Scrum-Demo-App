using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using NavigationTutorial.Core;
using NavigationTutorial.MVVM.ViewModel.ManageTasksOperationClass;
using NavigationTutorial.Services;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.ViewModel;

public class ManageTasksViewModel : Core.ViewModel
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
    
    // private ObservableCollection<User> _allUsers;
    //
    // public ObservableCollection<User> AllUsers
    // {
    //     get => _allUsers;
    // }
    //
    // private ObservableCollection<Task> _allTasks;
    //
    // public ObservableCollection<Task> AllTasks
    // {
    //     get => _allTasks;
    // }
    
    public ICommand NavigateToTaskView { get; set; }
    
    public ICommand NavigateToManageTasksView { get; set; }
    public ICommand AddTaskCommand { get; }
    public ICommand UpdateTaskCommand { get;  }
    
    public ICommand RemoveTaskCommand { get; }
    
    public CreateProjectTask CreateProjectTask { get; set; }
    public DeleteProjectTask DeleteProjectTask { get; set; }
    public UpdateProjectTask UpdateProjectTask { get; set; }
    
    public ManageTasksViewModel(INavigationService navigationService, CreateProjectTask cpt, DeleteProjectTask dpt, UpdateProjectTask upt)
    {
        Navigation = navigationService;
        NavigateToTaskView = new RelayCommands(o => {Navigation.NavigateTo<TaskViewModel>();}, o => true);
        NavigateToManageTasksView = new RelayCommands(o => {Navigation.NavigateTo<ManageTasksViewModel>();}, o => true);
        AddTaskCommand = new RelayCommand(AddTask);
        UpdateTaskCommand = new RelayCommand(UpdateTask);
        RemoveTaskCommand = new RelayCommand(RemoveTask);
        CreateProjectTask = cpt;
        DeleteProjectTask = dpt;
        UpdateProjectTask = upt;
        UserProjects = ((App)Application.Current).LoggedUser.Projects;
        Console.Out.WriteLine("Manage Tasks View Class Created");
        // LoadMembers();
        // LoadTasks();
        _scrumDbContext = new ScrumDbContext();
    }
    
    // public void LoadMembers()
    // {
    //     var userProjects = ((App)Application.Current).LoggedUser.Projects;
    //     var  project = userProjects.FirstOrDefault(p => p.Id == ((App)Application.Current).ProjectId);
    //     _allUsers = new ObservableCollection<User>(project.Users);
    // }
    //
    // public void LoadTasks()
    // {
    //     var userProjects = ((App)Application.Current).LoggedUser.Projects;
    //     var  project = userProjects.FirstOrDefault(p => p.Id == ((App)Application.Current).ProjectId);
    //     _allTasks = new ObservableCollection<Task>(project.ProjectTasks);
    // }
    


    private async void AddTask()
    {
        Task newTask = CreateProjectTask.CreateTask();
        System.Diagnostics.Debug.WriteLine("Utworzono nowy task");
        if (newTask != null)
        {
            _scrumDbContext.Tasks.Add(newTask);
            _scrumDbContext.SaveChanges();
            System.Diagnostics.Debug.WriteLine("Dodano task do bazy danych");
            //TODO Refresh User Task
            //await ((App)Application.Current).LoadUserData(((App)Application.Current).LoggedUser.Id);
            await ((App)Application.Current).UpdateUserData(((App)Application.Current).LoggedUser.Id);

            Console.Out.WriteLine("Task added successfully!");
            NavigateToManageTasksView.Execute(null);
        }
    }
    
    private async void UpdateTask()
    {
        Console.Out.WriteLine("Update Task");
        Task updatedTask = UpdateProjectTask.UpdateTask();
    
        if (updatedTask != null)
        {
            var oldTask = _scrumDbContext.Tasks.FirstOrDefault(t => t.Id == updatedTask.Id);
        
            if (oldTask != null)
            {
                oldTask.Name = updatedTask.Name;
                oldTask.Description = updatedTask.Description;
                oldTask.Points = updatedTask.Points;

                _scrumDbContext.Tasks.Update(oldTask);
                _scrumDbContext.SaveChanges();

                //TODO Refresh User Task
                //await ((App)Application.Current).LoadUserData(((App)Application.Current).LoggedUser.Id);
                await ((App)Application.Current).UpdateUserData(((App)Application.Current).LoggedUser.Id);
            
                Console.Out.WriteLine("Task updated successfully!");
                NavigateToManageTasksView.Execute(null);
            }
            else
            {
                Console.Out.WriteLine("Task not found!");
            }
        }
    }

    
    private async void RemoveTask()
    {
        Console.Out.WriteLine("Remove Task");
        Task removeTask = DeleteProjectTask.RemoveTask();
        if (removeTask != null)
        {
            Task temp = _scrumDbContext.Tasks.FirstOrDefault(t => t.Id == removeTask.Id);
            if (temp != null)
            {
                _scrumDbContext.Tasks.Remove(temp);
                _scrumDbContext.SaveChanges();

                //TODO Refresh User Task
                await ((App)Application.Current).UpdateUserData(((App)Application.Current).LoggedUser.Id);

                Console.Out.WriteLine("Task deleted successfully!");
                NavigateToManageTasksView.Execute(null);
            }
            else
            {
                Console.Out.WriteLine("Task not found!");
            }
        }
    }
}