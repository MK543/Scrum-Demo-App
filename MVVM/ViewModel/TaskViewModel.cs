using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using NavigationTutorial.Core;
using NavigationTutorial.Services;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.ViewModel;

public class TaskViewModel : Core.ViewModel
{
    private TaskListingViewModel _toDoTaskListingViewModel;
    private TaskListingViewModel _inProgressTaskListingViewModel;
    private TaskListingViewModel _toTestTaskListingViewModel;
    private TaskListingViewModel _completedTaskListingViewModel;

    //02.01.2024 KK
    private int _projectPointsSum;

    public int ProjectPointsSum
    {
        get => _projectPointsSum;
        set
        {
            _projectPointsSum = value;
            OnPropertyChanged(nameof(ProjectPointsSum));
        }
    }

    private string _selectedProjectManagerName;
    public string SelectedProjectManagerName
    {
        get => _selectedProjectManagerName;
        set
        {
            _selectedProjectManagerName = value;
            OnPropertyChanged(nameof(SelectedProjectManagerName));
        } 
    }

    private DateTime _creationDate;

    public DateTime CreationDate
    {
        get => _creationDate;
        set
        {
            _creationDate = value;
            OnPropertyChanged(nameof(CreationDate));
        }
    }

    private DateTime _finishDate;

    public DateTime FinishDate
    {
        get => _finishDate;
        set
        {
            _finishDate = value;
            OnPropertyChanged(nameof(FinishDate));
        }
        
    }
    
    private double _leftTime;

    public double LeftTime
    {
        get => _leftTime;
        set
        {
            _leftTime = value;
            OnPropertyChanged(nameof(LeftTime));
        }
    }
    //

    private int _toDoTasksCount;
    public int ToDoTasksCount
    {
        get => _toDoTasksCount;
        set
        {
            _toDoTasksCount = value;
            OnPropertyChanged(nameof(ToDoTasksCount));
        }
    }
    
    private int _toDoTasksPoints;
    public int ToDoTasksPoints
    {
        get => _toDoTasksPoints;
        set
        {
            _toDoTasksPoints = value;
            OnPropertyChanged(nameof(ToDoTasksPoints));
        }
    }
    
    private int _inProgressTasksCount;
    public int InProgressTasksCount
    {
        get => _inProgressTasksCount;
        set
        {
            _inProgressTasksCount = value;
            OnPropertyChanged(nameof(InProgressTasksCount));
        }
    }
    
    private int _inProgressTasksPoints;
    public int InProgressTasksPoints
    {
        get => _inProgressTasksPoints;
        set
        {
            _inProgressTasksPoints = value;
            OnPropertyChanged(nameof(InProgressTasksPoints));
        }
    }
    
    private int _toTestTasksCount;
    public int ToTestTasksCount
    {
        get => _toTestTasksCount;
        set
        {
            _toTestTasksCount = value;
            OnPropertyChanged(nameof(ToTestTasksCount));
        }
    }
    
    private int _toTestTasksPoints;
    public int ToTestTasksPoints
    {
        get => _toTestTasksPoints;
        set
        {
            _toTestTasksPoints = value;
            OnPropertyChanged(nameof(ToTestTasksPoints));
        }
    }
    
    private int _completedTasksCount;
    public int CompletedTasksCount
    {
        get => _completedTasksCount;
        set
        {
            _completedTasksCount = value;
            OnPropertyChanged(nameof(CompletedTasksCount));
        }
    }
    
    private int _completedTasksPoints;
    public int CompletedTasksPoints
    {
        get => _completedTasksPoints;
        set
        {
            _completedTasksPoints = value;
            OnPropertyChanged(nameof(CompletedTasksPoints));
        }
    }

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

    //Added 31.12.2023 - MK
    public List<Project> UserProjects { get; set; }

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
    
    public ICommand NavigateToManageTasksView { get; set; }
    
    //
    
    public TaskViewModel(INavigationService navigationService)
    {
        //Added 31.12.2023 - MK
        Navigation = navigationService;
        NavigateToManageTasksView = new RelayCommands(o => {Navigation.NavigateTo<ManageTasksViewModel>();}, o => true);
        UserProjects = ((App)Application.Current).LoggedUser.Projects;
        CurrentUser = ((App)Application.Current).LoggedUser;
        Console.Out.WriteLine(CurrentUser);
        
        var app = (App)Application.Current;
        if (UserProjects != null && UserProjects.Any())
        {
            LoadTasks();
            
            var selectedProject = UserProjects.FirstOrDefault(p => p.Id == app.ProjectId);
            CreationDate = selectedProject.CreationDate.ToLocalTime();
            FinishDate = selectedProject.FinishDate.ToLocalTime();
            LeftTime = Math.Ceiling((selectedProject.FinishDate - DateTime.Today).TotalDays);
            if (LeftTime < 0) LeftTime = 0;
            System.Diagnostics.Debug.WriteLine("Zostalo do konca projektu:");
            System.Diagnostics.Debug.WriteLine(Math.Floor(LeftTime));
            var manager = selectedProject.Users.FirstOrDefault(u => u.Id == selectedProject.ProjectCreatorId);
            SelectedProjectManagerName = manager.FirstName + manager.LastName;;
            System.Diagnostics.Debug.WriteLine(SelectedProjectManagerName);
        }
        else
        {
            SelectedProjectName = null;
        }
        SelectedProjectName = UserProjects.FirstOrDefault(project => project.Id == ((App)Application.Current).ProjectId)
            .ProjectName;
        //
        
        app.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(app.ProjectId))
            {
                LoadTasks();
                System.Diagnostics.Debug.WriteLine(SelectedProjectManagerName);
            }
        };
    }
    
    public List<Task> ProjectTasks { get; set; }
    
    public void LoadTasks()
    {
        TaskListingViewModel listToDo = new TaskListingViewModel(TaskType.TO_DO);
        TaskListingViewModel listInProgress = new TaskListingViewModel(TaskType.IN_PROGRESS);
        TaskListingViewModel listToTest = new TaskListingViewModel(TaskType.TO_TEST);
        TaskListingViewModel listCompleted = new TaskListingViewModel(TaskType.COMPLETED);
        
        var userProjects = ((App)Application.Current).LoggedUser.Projects;
        var project = userProjects.FirstOrDefault(p => p.Id == ((App)Application.Current).ProjectId);
        ProjectTasks = project.ProjectTasks;
        System.Diagnostics.Debug.WriteLine(project.ProjectName);
        //Added 02.01.2024 KK
        CreationDate = project.CreationDate.ToLocalTime();
        FinishDate = project.FinishDate.ToLocalTime();
        LeftTime = Math.Ceiling((project.FinishDate - DateTime.Today).TotalDays);
        if (LeftTime < 0) LeftTime = 0;
        System.Diagnostics.Debug.WriteLine("Zostalo do konca projektu:");
        System.Diagnostics.Debug.WriteLine(Math.Floor(LeftTime));
        var managerId = project.ProjectCreatorId;
        var manager = project.Users.FirstOrDefault(u => u.Id == managerId);
        System.Diagnostics.Debug.WriteLine(manager.FirstName);
        SelectedProjectManagerName = manager.FirstName + manager.LastName;
        System.Diagnostics.Debug.WriteLine(manager.FirstName);
        //
        


        foreach (var task in ProjectTasks)
        {
            switch (task.TaskType)
            {
                case TaskType.TO_DO:
                    listToDo.AddTask(task);
                    break;
                case TaskType.IN_PROGRESS:
                    listInProgress.AddTask(task);
                    break;
                case TaskType.TO_TEST:
                    listToTest.AddTask(task);
                    break;
                case TaskType.COMPLETED:
                    listCompleted.AddTask(task);
                    break;
            }    
        }
        
        ChangeToDoList(listToDo);
        ChangeInProgressList(listInProgress);
        ChangeToTestList(listToTest);
        ChangeCompletedList(listCompleted);
        
        //02.01.2024 KK
        ProjectPointsSum = ToDoTasksPoints + InProgressTasksPoints + ToTestTasksPoints + CompletedTasksPoints;
        //
    }
    
    
    public TaskListingViewModel ToDoTaskListingViewModel
    {
        get => _toDoTaskListingViewModel;
        set
        {
            _toDoTaskListingViewModel = value;
            _toDoTaskListingViewModel.ListChanged += UpdateToDoTasksCount;
            OnPropertyChanged(nameof(ToDoTaskListingViewModel));
        }
    }
    private void UpdateToDoTasksCount(object sender, EventArgs e)
    {
        ToDoTasksCount = ToDoTaskListingViewModel.ListLength;
        ToDoTasksPoints = ToDoTaskListingViewModel.ListPoints;
    }
    public TaskListingViewModel InProgressTaskListingViewModel
    {
        get => _inProgressTaskListingViewModel;
        set
        {
            _inProgressTaskListingViewModel = value;
            _inProgressTaskListingViewModel.ListChanged += UpdateInProgressTasksCount;
            OnPropertyChanged(nameof(InProgressTaskListingViewModel));
        }
    }
    private void UpdateInProgressTasksCount(object sender, EventArgs e)
    {
        InProgressTasksCount = InProgressTaskListingViewModel.ListLength;
        InProgressTasksPoints = InProgressTaskListingViewModel.ListPoints;
    }
    public TaskListingViewModel ToTestTaskListingViewModel
    {
        get => _toTestTaskListingViewModel;
        set
        {
            _toTestTaskListingViewModel = value;
            _toTestTaskListingViewModel.ListChanged += UpdateToTestTasksCount;
            OnPropertyChanged(nameof(ToTestTaskListingViewModel));
        }
    }
    private void UpdateToTestTasksCount(object sender, EventArgs e)
    {
        ToTestTasksCount = ToTestTaskListingViewModel.ListLength;
        ToTestTasksPoints = ToTestTaskListingViewModel.ListPoints;
    }
    public TaskListingViewModel CompletedTaskListingViewModel
    {
        get => _completedTaskListingViewModel;
        set
        {
            _completedTaskListingViewModel = value;
            _completedTaskListingViewModel.ListChanged += UpdateCompletedTasksCount;
            OnPropertyChanged(nameof(CompletedTaskListingViewModel));
        }
    }
    private void UpdateCompletedTasksCount(object sender, EventArgs e)
    {
        CompletedTasksCount = CompletedTaskListingViewModel.ListLength;
        CompletedTasksPoints = CompletedTaskListingViewModel.ListPoints;
    }

    public void ChangeToDoList(TaskListingViewModel newList)
    {
        ToDoTaskListingViewModel = newList;
        ToDoTasksCount = ToDoTaskListingViewModel.ListLength;
        ToDoTasksPoints = ToDoTaskListingViewModel.ListPoints;
    }
    public void ChangeInProgressList(TaskListingViewModel newList)
    {
        InProgressTaskListingViewModel = newList;
        InProgressTasksCount = InProgressTaskListingViewModel.ListLength;
        InProgressTasksPoints = InProgressTaskListingViewModel.ListPoints;
    }
    public void ChangeToTestList(TaskListingViewModel newList)
    {
        ToTestTaskListingViewModel = newList;
        ToTestTasksCount = ToTestTaskListingViewModel.ListLength;
        ToTestTasksPoints = ToTestTaskListingViewModel.ListPoints;
    }
    public void ChangeCompletedList(TaskListingViewModel newList)
    {
        CompletedTaskListingViewModel = newList;
        CompletedTasksCount = CompletedTaskListingViewModel.ListLength;
        CompletedTasksPoints = CompletedTaskListingViewModel.ListPoints;
    }
}