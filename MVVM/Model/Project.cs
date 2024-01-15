using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.JavaScript;

namespace ScrumApp.MVVM.Model;

public class Project
{
    public int Id { get; set; }
    public string ProjectName { get; set; }
    public string ProjectDescription { get; set; }
    public List<User> Users { get; set; } = new List<User>();
    public List<Task> ProjectTasks { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime FinishDate { get; set; }
    public int ProjectCreatorId { get; set; }

    [NotMapped]
    public User ProjectManager { get; set; }
    
    public Project(string projectName, string projectDescription, int projectCreatorId, DateTime finishDate)
    {
        ProjectName = projectName;
        ProjectDescription = projectDescription;
        //Users = new List<User>();
        ProjectTasks = new List<Task>();
        ProjectCreatorId = projectCreatorId;
        CreationDate = DateTime.Today.ToUniversalTime()
            .AddHours(DateTime.UtcNow.Hour + 1)
            .AddMinutes(DateTime.UtcNow.Minute)
            .AddSeconds(DateTime.UtcNow.Second);
        FinishDate = finishDate.Date.ToUniversalTime()
            .AddHours(finishDate.Hour)
            .AddMinutes(finishDate.Minute)
            .AddSeconds(finishDate.Second);
    }

    public override bool Equals(object? obj)
    {
        return obj is Project project && ProjectName == project.ProjectName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((ProjectName));
    }
    
    public override string ToString()
    {
        return $"{ProjectName}";
    }
}