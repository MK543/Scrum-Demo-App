using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.View;

public partial class TaskListingView : UserControl
{
    public static readonly DependencyProperty IncomingTaskProperty =
        DependencyProperty.Register("IncomingTask", typeof(object), typeof(TaskListingView), 
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    
    public object IncomingTask
    {
        get { return (object)GetValue(IncomingTaskProperty); }
        set { SetValue(IncomingTaskProperty, value); }
    }
    
    public static readonly DependencyProperty RemovedTaskProperty =
        DependencyProperty.Register("RemovedTask", typeof(object), typeof(TaskListingView),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    
    public object RemovedTask
    {
        get { return (object)GetValue(RemovedTaskProperty); }
        set { SetValue(RemovedTaskProperty, value); }
    }
    
    public static readonly DependencyProperty TaskDropCommandProperty =
        DependencyProperty.Register("TaskDropCommand", typeof(ICommand), typeof(TaskListingView), 
            new PropertyMetadata(null));
    
    public ICommand TaskDropCommand
    {
        get { return (ICommand)GetValue(TaskDropCommandProperty); }
        set { SetValue(TaskDropCommandProperty, value); }
    }
    
    public static readonly DependencyProperty TaskRemovedCommandProperty =
        DependencyProperty.Register("TaskRemovedCommand", typeof(ICommand), typeof(TaskListingView), 
            new PropertyMetadata(null));

    public ICommand TaskRemovedCommand
    {
        get { return (ICommand)GetValue(TaskRemovedCommandProperty); }
        set { SetValue(TaskRemovedCommandProperty, value); }
    }
    
    public static readonly DependencyProperty TaskInsertedCommandProperty =
        DependencyProperty.Register("TaskInsertedCommand", typeof(ICommand), typeof(TaskListingView), 
            new PropertyMetadata(null));

    public ICommand TaskInsertedCommand
    {
        get { return (ICommand)GetValue(TaskInsertedCommandProperty); }
        set { SetValue(TaskInsertedCommandProperty, value); }
    }
    
    public static readonly DependencyProperty InsertedTaskProperty =
        DependencyProperty.Register("InsertedTask", typeof(object), typeof(TaskListingView),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public object InsertedTask
    {
        get { return (object)GetValue(InsertedTaskProperty); }
        set { SetValue(InsertedTaskProperty, value); }
    }
    
    public static readonly DependencyProperty TargetTaskProperty =
        DependencyProperty.Register("TargetTask", typeof(object), typeof(TaskListingView), 
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public object TargetTask
    {
        get { return (object)GetValue(TargetTaskProperty); }
        set { SetValue(TargetTaskProperty, value); }
    }

    public TaskListingView()
    {
        InitializeComponent();
    }
    
    private void Task_MouseMove(object sender, MouseEventArgs e)
    {
        if(e.LeftButton == MouseButtonState.Pressed &&
           sender is FrameworkElement frameworkElement)
        {
            object task = frameworkElement.DataContext;

            DragDropEffects dragDropResult = DragDrop.DoDragDrop(frameworkElement, 
                new DataObject(DataFormats.Serializable, task), 
                DragDropEffects.Move);

            if(dragDropResult == DragDropEffects.None)
            {
                AddTask(task);
            }
        }
    }
    
    private void Task_DragOver(object sender, DragEventArgs e)
    {
        if (TaskInsertedCommand?.CanExecute(null) ?? false)
        {
            if(sender is FrameworkElement element)
            {
                TargetTask = element.DataContext;
                InsertedTask = e.Data.GetData(DataFormats.Serializable);

                TaskInsertedCommand?.Execute(null);
            }
        }
    }
    
    private void TaskList_DragOver(object sender, DragEventArgs e)
    {
        object task = e.Data.GetData(DataFormats.Serializable);
        AddTask(task);
    }
    
    private void AddTask(object task)
    {
        if (TaskDropCommand?.CanExecute(null) ?? false)
        {
            IncomingTask = task;
            TaskDropCommand?.Execute(null);
        }
    }
    
    private void TaskList_DragLeave(object sender, DragEventArgs e)
    {
        HitTestResult result = VisualTreeHelper.HitTest(lvItems, e.GetPosition(lvItems));

        if(result == null)
        {
            if (TaskRemovedCommand?.CanExecute(null) ?? false)
            {
                RemovedTask = e.Data.GetData(DataFormats.Serializable);
                TaskRemovedCommand?.Execute(null);
            }
        }    
    }

    private void Task_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is ListViewItem item && item.DataContext is Task selectedTask)
        {
            string taskInfo = $"Nazwa: {selectedTask.Name}\nOpis: {selectedTask.Description}\nTyp: {selectedTask.TaskType}";

            MessageBox.Show(taskInfo, "Szczegóły zadania", MessageBoxButton.OK);
        }
    }
}