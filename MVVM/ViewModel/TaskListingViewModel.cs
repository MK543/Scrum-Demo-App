using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using MVVMEssentials.ViewModels;
using NavigationTutorial.Commands;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.ViewModel;

public class TaskListingViewModel : ViewModelBase
{
    public TaskType ListType;
    public int ListLength;
    public int ListPoints;
    public event EventHandler ListChanged;
    
    private readonly ObservableCollection<Task> _taskViewModels;

    public IEnumerable<Task> TaskViewModels => _taskViewModels;

    private Task _incomingTaskViewModel;
    public Task IncomingTaskViewModel
    {
        get
        {
            return _incomingTaskViewModel;
        }
        set
        {
            _incomingTaskViewModel = value;
            OnPropertyChanged(nameof(IncomingTaskViewModel));
        }
    }    
    
    private Task _removedTaskViewModel;
    public Task RemovedTaskViewModel
    {
        get
        {
            return _removedTaskViewModel;
        }
        set
        {
            _removedTaskViewModel = value;
            OnPropertyChanged(nameof(RemovedTaskViewModel));
        }
    }    
    
    private Task _insertedTaskViewModel;
    public Task InsertedTaskViewModel
    {
        get
        {
            return _insertedTaskViewModel;
        }
        set
        {
            _insertedTaskViewModel = value;
            OnPropertyChanged(nameof(InsertedTaskViewModel));
        }
    }   
    private Task _targetTaskViewModel;
    public Task TargetTaskViewModel
    {
        get
        {
            return _targetTaskViewModel;
        }
        set
        {
            _targetTaskViewModel = value;
            OnPropertyChanged(nameof(TargetTaskViewModel));
        }
    }
    
    public ICommand TaskReceivedCommand { get; }
    public ICommand TaskRemovedCommand { get; }
    public ICommand TaskInsertedCommand { get; }

    public TaskListingViewModel(TaskType listType)
    {
        _taskViewModels = new ObservableCollection<Task>();

        ListLength = 0;
        ListPoints = 0;
        ListType = listType;

        TaskReceivedCommand = new TaskReceivedCommand(this);
        TaskRemovedCommand = new TaskRemovedCommand(this);
        TaskInsertedCommand = new TaskInsertedCommand(this);
    }

    public void AddTask(Task item)
    {
        if (!_taskViewModels.Contains(item))
        {
            _taskViewModels.Add(item);
            ListLength = _taskViewModels.Count;
            ListPoints = _taskViewModels.Sum(task => task.Points);
            OnListChanged();
            System.Diagnostics.Debug.WriteLine("Po dodaniu jest:");
            System.Diagnostics.Debug.WriteLine(ListPoints);
        }
    }
    
    public void InsertTask(Task insertedTask, Task targetTask)
    {
        if (insertedTask == targetTask)
        {
            return;
        }

        int oldIndex = _taskViewModels.IndexOf(insertedTask);
        int nextIndex = _taskViewModels.IndexOf(targetTask);

        if (oldIndex != -1 && nextIndex != -1)
        {
            _taskViewModels.Move(oldIndex, nextIndex);
        }
    }

    public void RemoveTask(Task item)
    {
        _taskViewModels.Remove(item);
        ListLength = _taskViewModels.Count;
        ListPoints = _taskViewModels.Sum(task => task.Points);
        OnListChanged();
        System.Diagnostics.Debug.WriteLine("Po odjeciu jest:");
        System.Diagnostics.Debug.WriteLine(ListPoints);
    }
    
    protected virtual void OnListChanged()
    {
        ListChanged?.Invoke(this, EventArgs.Empty);
    }

}