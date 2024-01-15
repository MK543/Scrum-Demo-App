using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.View;

public partial class ProjectsWindow : Window
{
    public ProjectsWindow()
    {
        var app = (App)Application.Current;
        app.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(app.ProjectId) || args.PropertyName == nameof(app.ProjectName))
            {
                changeProjectName();
            }
        };
        InitializeComponent();
    }
    

    public List<Project> UserProjects { get; set; }
    
    public Project Project;
    public void changeProjectName()
    {
        UserProjects = ((App)Application.Current).LoggedUser.Projects;
        Project = UserProjects.FirstOrDefault(p => p.Id == ((App)Application.Current).ProjectId);
        ProjectNameTextBlock.Text = Project.ProjectName;
    }

    private void DragWindow(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }
}