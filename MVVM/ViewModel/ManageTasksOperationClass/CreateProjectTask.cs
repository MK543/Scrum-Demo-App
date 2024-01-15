using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Mime;
using System.Windows;
using System.Windows.Data;
using NavigationTutorial.Core;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.ViewModel.ManageTasksOperationClass;

public class CreateProjectTask : ObservableObject
{
    private string _taskName;

    public string TaskName
    {
        get => _taskName;
        set
        {
            _taskName = value;
            OnPropertyChanged(nameof(TaskName));
        }
    }
    
    private string _taskDesc;
    
    public string TaskDesc
    {
        get => _taskDesc;
        set
        {
            _taskDesc = value;
            OnPropertyChanged(nameof(TaskDesc));
        }
    }

    private string _taskPoints;
    
    public string TaskPoints
    {
        get => _taskPoints;
        set
        {
            _taskPoints = value;
            OnPropertyChanged(nameof(TaskPoints));
        }
    }
    
    private User _selectedUser;
    
    public User SelectedUser
    {
        get => _selectedUser;
        set
        {
            _selectedUser = value;
            OnPropertyChanged(nameof(SelectedUser));
        }
    }
    
    private string _searchUser;
    public string SearchUser
    {
        get => _searchUser;
        set
        {
            _searchUser = value;
            OnPropertyChanged(nameof(SearchUser));
        }
    }
    
    public ObservableCollection<User> Users { get; set; }
    

    private ObservableCollection<User> _allUsers;
    
    public ObservableCollection<User> AllUsers
    {
        get => _allUsers;
    }

    private string _invalidTaskNameLabel;
    private string _invalidTaskDescLabel;
    private string _invalidTaskPointsLabel;
    private string _invalidMemberLabel;
    
    public string InvalidTaskNameLabel { 
        get => _invalidTaskNameLabel;
        set
        {
            _invalidTaskNameLabel = value;
            OnPropertyChanged();
        } 
    }

    public string InvalidTaskDescLabel
    {
        get => _invalidTaskDescLabel; 
        set
        {
            _invalidTaskDescLabel = value;
            OnPropertyChanged();
        } 
    }

    public string InvalidTaskPointsLabel
    {
        get => _invalidTaskPointsLabel; 
        set
        {
            _invalidTaskPointsLabel = value;
            OnPropertyChanged();
        } 
    }

    public string InvalidMemberLabel
    {
        get => _invalidMemberLabel;
        set
        {
            _invalidMemberLabel = value;
            OnPropertyChanged();
        }
    }
    

    public CreateProjectTask()
    {
        //PropertyChanged += UpdateFilteredUsers;
        Console.Out.WriteLine($"CreateProject Created");
        LoadMembers();
    }
    
    public void LoadMembers()
    {
        var userProjects = ((App)Application.Current).LoggedUser.Projects;
        var  project = userProjects.FirstOrDefault(p => p.Id == ((App)Application.Current).ProjectId);
        _allUsers = new ObservableCollection<User>(project.Users);
        Users = new ObservableCollection<User>(_allUsers);
    }
    

    // private void UpdateFilteredUsers(object? sender, PropertyChangedEventArgs e)
    // {
    //
    //     if (e.PropertyName == "SelectedUser" && SearchUser != null)
    //     {
    //         var receivedText = SearchUser.Split(" ");
    //         var firstName = receivedText[0];
    //         var lastName = receivedText.Length > 1 ? receivedText[1] : ""; 
    //         var filteredUsers = AllUsers
    //             .Where(user =>
    //                 user.FirstName.Contains(firstName, System.StringComparison.OrdinalIgnoreCase) &&
    //                 user.LastName.Contains(lastName, System.StringComparison.OrdinalIgnoreCase))
    //             .ToList();
    //
    //         Users = new ObservableCollection<User>(filteredUsers);
    //         OnPropertyChanged(nameof(Users));
    //     }
    //    
    // }
    
    private bool Validate()
    {
        InvalidTaskNameLabel = null;
        InvalidTaskDescLabel = null;
        InvalidTaskPointsLabel = null;
        InvalidMemberLabel = null;
        // Task Points Field Validation
        bool taskPointsValid = true;
        if (TaskPoints != null && int.TryParse(TaskPoints, out int result))
        {
            if (result <= 0)
            {
                Console.Out.WriteLine(result);
                taskPointsValid = false;
                InvalidTaskPointsLabel = "Positive integer required";
            }
        }
        else
        {
            taskPointsValid = false;
            InvalidTaskPointsLabel = "Field validation error";
        }
        //Task Name Field Validation
        bool taskNameValid = !string.IsNullOrWhiteSpace(TaskName) && TaskName.Length <= 10;
        if (!taskNameValid) InvalidTaskNameLabel = $"Name must have less than\n10 characters and not empty";

        //Task Desc Field Validation
        bool taskDescValid = !string.IsNullOrWhiteSpace(TaskDesc) && TaskDesc.Length <= 50;
        if (!taskDescValid) InvalidTaskDescLabel = $"Name must have less than\n50 characters and not empty";

        //Assigned User Field Validation
        bool assignedUser = SelectedUser != null;
        if (!assignedUser) InvalidMemberLabel = $"Member must be selected";
        
        Console.Out.WriteLine($" {taskPointsValid} {taskNameValid} {taskDescValid} {assignedUser}");

        if (taskPointsValid && taskNameValid && taskDescValid && assignedUser) return true;
        return false;

    }
    public Task CreateTask()
    {
        if (Validate())
        {
            Task newTask = new Task(TaskName, TaskDesc, TaskType.TO_DO, ((App)Application.Current).ProjectId, int.Parse(TaskPoints), SelectedUser.Id);
            return newTask;
        }
        return null;
    }
    
}