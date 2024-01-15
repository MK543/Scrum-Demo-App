using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Microsoft.VisualBasic.CompilerServices;
using NavigationTutorial.Core;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.ViewModel.ManageProjectsOperationClass;

public class UpdateProject : ObservableObject
{
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

    private Project _selectedProject;
    
    public Project SelectedProject
    {
        get => _selectedProject;
        set
        {
            _selectedProject = value;
            ProjectName = _selectedProject.ProjectName;
            ProjectDesc = _selectedProject.ProjectDescription;
            FinishDate = _selectedProject.FinishDate.ToLocalTime();
            OnPropertyChanged(nameof(SelectedProject));
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
    
    private string _searchProject;
    public string SearchProject
    {
        get => _searchProject;
        set
        {
            _searchProject = value;
            OnPropertyChanged(nameof(SearchProject));
        }
    }
    
    private ObservableCollection<User> _allUsers;
    
    public ObservableCollection<User> AllUsers
    {
        get => _allUsers;
    }
    
    private ObservableCollection<Project> _allProjects;
    
    public ObservableCollection<Project> AllProjects
    {
        get => _allProjects;
    }
    
    public ObservableCollection<User> Users { get; set; }
    
    public ObservableCollection<Project> Projects { get; set; }

    private string _invalidProjectSelectLabel;
    private string _invalidProjectNameLabel;
    private string _invalidProjectDescLabel;
    private string _invalidProjectDateLabel;
    private string _projectOldDate;

    public string InvalidProjectSelectLabel
    {
        get => _invalidProjectSelectLabel;
        set
        {
            _invalidProjectSelectLabel = value;
            OnPropertyChanged();
        }
    }

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

    public string InvalidProjectDateLabel
    {
        get => _invalidProjectDateLabel;
        set
        {
            _invalidProjectDateLabel = value;
            OnPropertyChanged();
        }
    }

    public string ProjectOldDate
    {
        get => _projectOldDate;
        set
        {
            _projectOldDate = value;
            OnPropertyChanged();
        }
    }


    public UpdateProject()
    {
        LoadProjects();

        //ustawianie domyslnej wartosci daty na obecna
        FinishDate = DateTime.Today.ToUniversalTime()
            .AddHours(DateTime.UtcNow.Hour + 2)
            .AddMinutes(DateTime.UtcNow.Minute)
            .AddSeconds(DateTime.UtcNow.Second);
        

        
        Console.Out.WriteLine($"UpdateProject Created");
        System.Diagnostics.Debug.WriteLine("UpdateProject Created");
        // PropertyChanged += UpdateFilteredUsers;
        // PropertyChanged += UpdateFilteredTasks;
        PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName.Equals("SelectedProject") && SelectedProject != null)
                ProjectOldDate = SelectedProject.FinishDate.ToString();
        };
    }
    
    public void LoadProjects()
    {
        var loggedUserId = ((App)Application.Current).LoggedUser.Id;
        var userProjects = ((App)Application.Current).LoggedUser.Projects.Where(project => project.ProjectCreatorId == loggedUserId).ToList();
        _allProjects = new ObservableCollection<Project>(userProjects);
        Projects = new ObservableCollection<Project>(_allProjects);
    }
    
    
    private bool Validate()
    {
        InvalidProjectSelectLabel = null;
        InvalidProjectNameLabel = null;
        InvalidProjectDescLabel = null;
        InvalidProjectDateLabel = null;
        
        //Project Name Field Validation
        bool projectNameValid = !string.IsNullOrWhiteSpace(ProjectName) && ProjectName.Length <= 20 && ProjectName.Length >= 3;
        if (!projectNameValid) InvalidProjectNameLabel = "Project name must have\nbetween 3 and 20 characters";

        //Project Desc Field Validation
        bool projectDescValid = !string.IsNullOrWhiteSpace(ProjectDesc) && ProjectDesc.Length <= 300;
        if (!projectDescValid) InvalidProjectDescLabel = "Project description must\nhave 300 or less characters";
        
        //Old Project Field Validation and New Project Date Validation
        bool oldProject = SelectedProject != null;
        bool newProjectDate = true;
        if (!oldProject) InvalidProjectSelectLabel = "Project must be selected";
        else
        {
            newProjectDate = FinishDate > SelectedProject.FinishDate;
            if (!newProjectDate) InvalidProjectDateLabel = "New date must be set\nlater than the old one";
        }
        Console.Out.WriteLine($" {oldProject} {projectNameValid} {projectDescValid} {newProjectDate}");

        if (oldProject && projectNameValid && projectDescValid && oldProject && newProjectDate) return true;
        return false;

    }
    public Project UpdateExistingProject()
    {
        if (Validate())
        {
            Project projectToUpdate = SelectedProject;
            projectToUpdate.ProjectDescription = ProjectDesc;
            projectToUpdate.ProjectName = ProjectName;
            projectToUpdate.FinishDate = FinishDate.Date.ToUniversalTime()
                .AddHours(FinishDate.Hour)
                .AddMinutes(FinishDate.Minute)
                .AddSeconds(FinishDate.Second);
            return projectToUpdate;
        }
        return null;
    }
    
}