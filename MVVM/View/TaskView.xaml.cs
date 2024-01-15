using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.View;

public partial class TaskView : UserControl
{
    public TaskView()
    {
        InitializeComponent();
    }

    private void SwitchProjectsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox && comboBox.SelectedItem is Project selectedProject)
        {
                int selectedIndex = comboBox.SelectedIndex;
                List<Project> userProjects = ((App)Application.Current).LoggedUser.Projects;
                ((App)Application.Current).ProjectId = userProjects[selectedIndex].Id;
        }
    }
    
}