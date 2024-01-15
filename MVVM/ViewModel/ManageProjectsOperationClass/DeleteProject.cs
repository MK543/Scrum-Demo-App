using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.ViewModel.ManageProjectsOperationClass;

public class DeleteProject : Core.ViewModel
{
    private Project _selectedProject;
    
    public Project SelectedProject
    {
        get => _selectedProject;
        set
        {
            _selectedProject = value;
            OnPropertyChanged(nameof(SelectedProject));
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
    
    private ObservableCollection<Project> _allProjects;
    public ObservableCollection<Project> AllProjects
    {
        get => _allProjects;
    }

    private string _invalidProjectSelectLabel;

    public string InvalidProjectSelectLabel
    {
        get => _invalidProjectSelectLabel;
        set
        {
            _invalidProjectSelectLabel = value;
            OnPropertyChanged();
        }  
    }


    public void LoadProjects()
    {
        var loggedUserId = ((App)Application.Current).LoggedUser.Id;
        var userProjects = ((App)Application.Current).LoggedUser.Projects.Where(project => project.ProjectCreatorId == loggedUserId).ToList();
        _allProjects = new ObservableCollection<Project>(userProjects);
        Projects = new ObservableCollection<Project>(_allProjects);
    }
    
    public ObservableCollection<Project> Projects { get; set; }
    
    public DeleteProject()
    {
        LoadProjects();
        Console.Out.WriteLine($"DeleteProject Created");
        PropertyChanged += UpdateFilteredProjects;
    }
    
    private void UpdateFilteredProjects(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "SearchProject" && SearchProject != null)
        {
            var filteredProjects = AllProjects
                .Where(project =>
                    project.ProjectName.Contains(SearchProject, StringComparison.OrdinalIgnoreCase))
                .ToList();
            Projects = new ObservableCollection<Project>(filteredProjects);
            OnPropertyChanged(nameof(Projects));
        }
       
    }
    
    private bool Validate()
    {
        InvalidProjectSelectLabel = SelectedProject != null ? "" : "Project must be selected"; 
        return SelectedProject != null;

    }
    public Project RemoveProject()
    {
        if (Validate())
        {
            return SelectedProject;
        }
        return null;
    }
    
}