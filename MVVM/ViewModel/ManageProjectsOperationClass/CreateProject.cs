using System;
using System.Diagnostics.Tracing;
using System.Windows;
using NavigationTutorial.Core;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.ViewModel.ManageProjectsOperationClass;

public class CreateProject : ObservableObject
{
    private string _projectName;

    public string ProjectName
    {
        get => _projectName;
        set
        {
            _projectName = value;
            OnPropertyChanged(nameof(ProjectName));
        }
    }
    
    private string _projectDesc;
    
    public string ProjectDesc
    {
        get => _projectDesc;
        set
        {
            _projectDesc = value;
            OnPropertyChanged(nameof(ProjectDesc));
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

    private string _invalidProjectNameLabel;
    private string _invalidProjectDescLabel;
    private string _invalidDateLabel;

    public string InvalidProjectNameLabel
    {
        get => _invalidProjectNameLabel;
        set
        {
            _invalidProjectNameLabel = value;
            OnPropertyChanged();
        }
    }
    public string InvalidProjectDescLabel
    {
        get => _invalidProjectDescLabel;
        set
        {
            _invalidProjectDescLabel = value;
            OnPropertyChanged();
        }
    }
    public string InvalidDateLabel
    {
        get => _invalidDateLabel;
        set
        {
            _invalidDateLabel = value;
            OnPropertyChanged();
        }
    }
    public CreateProject()
    {
        Console.Out.WriteLine("CreateProject Created");
        FinishDate = DateTime.Today.ToUniversalTime()
            .AddHours(DateTime.UtcNow.Hour + 2)
            .AddMinutes(DateTime.UtcNow.Minute)
            .AddSeconds(DateTime.UtcNow.Second);
    }
    
    private bool Validate()
    {
        InvalidProjectNameLabel = "";
        InvalidProjectDescLabel = "";
        InvalidDateLabel = "";
        
        //Project Name Field Validation
        bool projectNameValid = !string.IsNullOrWhiteSpace(ProjectName) && ProjectName.Length <= 20 && ProjectName.Length >= 3;
        if (!projectNameValid) InvalidProjectNameLabel = "Project name must have\nbetween 3 and 20 characters";

        //Project Desc Field Validation
        bool projectDescValid = !string.IsNullOrWhiteSpace(ProjectDesc) && ProjectDesc.Length <= 300;
        if (!projectNameValid) InvalidProjectDescLabel = "Project description must\nhave 300 or less characters";
        
        //Project Date Field Validation
        bool projectDateValid = FinishDate != null && FinishDate > DateTime.Now.AddDays(7);;
        if (!projectDateValid) InvalidDateLabel = "Finish date must at\nleast one week later";
        
        
        if (projectNameValid && projectDescValid && projectDateValid) return true;
        return false;

    }
    public Project CreateNewProject()
    {
        if (Validate())
        {
            var loggedUserId = ((App)Application.Current).LoggedUser.Id;
            System.Diagnostics.Debug.Write("LoggedUserID");
            System.Diagnostics.Debug.Write(loggedUserId);
            Project newProject = new Project(ProjectName, ProjectDesc, loggedUserId, FinishDate);
            return newProject;
        }
        return null;
    }
    
}