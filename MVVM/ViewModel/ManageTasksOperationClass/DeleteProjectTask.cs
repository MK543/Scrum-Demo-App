using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.ViewModel.ManageTasksOperationClass;

public class DeleteProjectTask : Core.ViewModel
{
    private Task _selectedTask;
    
    public Task SelectedTask
    {
        get => _selectedTask;
        set
        {
            _selectedTask = value;
            OnPropertyChanged(nameof(SelectedTask));
        }
    }
    
    private string _searchTask;
    public string SearchTask
    {
        get => _searchTask;
        set
        {
            _searchTask = value;
            OnPropertyChanged(nameof(SearchTask));
        }
    }
    
    public ObservableCollection<Task> AllTasks
    {
        get => _allTasks;
    }
    
    private ObservableCollection<Task> _allTasks;

    private string _invalidTaskSelectLabel;
    
    public string InvalidTaskSelectLabel
    {
        get => _invalidTaskSelectLabel;
        set
        {
            _invalidTaskSelectLabel = value;
            OnPropertyChanged();
        }
    }
    
    public void LoadTasks()
    {
        var userProjects = ((App)Application.Current).LoggedUser.Projects;
        var  project = userProjects.FirstOrDefault(p => p.Id == ((App)Application.Current).ProjectId);
        _allTasks = new ObservableCollection<Task>(project.ProjectTasks);
        Tasks = new ObservableCollection<Task>(_allTasks);
    }
    
    public ObservableCollection<Task> Tasks { get; set; }

    
    public DeleteProjectTask()
    {
        LoadTasks();
        Console.Out.WriteLine($"DeleteProject Created");
        PropertyChanged += UpdateFilteredTasks;
    }
    
    private void UpdateFilteredTasks(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "SearchTask" && SearchTask != null)
        {
            var filteredTasks = AllTasks
                .Where(task =>
                    task.Name.Contains(SearchTask, StringComparison.OrdinalIgnoreCase))
                .ToList();
            Tasks = new ObservableCollection<Task>(filteredTasks);
            OnPropertyChanged(nameof(Tasks));
        }
       
    }
    
    private bool Validate()
    {
        InvalidTaskSelectLabel = SelectedTask != null ? null : "Task must be selected";
        return SelectedTask != null;

    }
    public Task RemoveTask()
    {
        if (Validate())
        {
            return SelectedTask;
        }
        return null;
    }
    
}