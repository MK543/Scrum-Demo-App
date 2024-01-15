using System.Collections.Generic;
using MVVMEssentials.ViewModels;

namespace ScrumApp.MVVM.Model;

public class Task : ViewModelBase
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public TaskType TaskType { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }
    public int Points { get; set; }
    public User AssignedUser { get; set; }
    public int AssignedUserId { get; set; }
    
    public Task(string name, string description, TaskType taskType, int projectId, int points, int assignedUserId)
    {
        Name = name;
        Description = description;
        TaskType = taskType;
        ProjectId = projectId;
        Points = points;
        AssignedUserId = assignedUserId;
    }
    
    public override string ToString()
    {
        return $"{Name}";
    }
}