using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.View;

public partial class ProjectsView : UserControl
{
    public ProjectsView()
    {
        InitializeComponent();
    }
    
    private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is ListBox listBox)
        {
            if (listBox.SelectedItem is Project selectedProject)
            {
                int selectedIndex = listBox.SelectedIndex;
                List<Project> userProjects = ((App)Application.Current).LoggedUser.Projects;
                ((App)Application.Current).ProjectId = userProjects[selectedIndex].Id;
            }
        }
    }
}