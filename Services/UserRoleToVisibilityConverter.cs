using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.Services;

public class UserRoleToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        User user = value is User ? (User)value : null;
        String view = parameter is String ? (String)parameter : null;
        if (parameter != null && user != null)
        {
            if (user.JobTitle == JobTitleName.ProjectManager && !view.Equals("TaskView")) return Visibility.Visible;
            else if (user.JobTitle == JobTitleName.ProjectManager && user.Projects.Count > 0) return Visibility.Visible;
            else if (user.JobTitle == JobTitleName.TeamLeader && !view.Equals("TaskView")) return Visibility.Collapsed;
            else if (user.JobTitle == JobTitleName.TeamLeader && user.Projects.Count > 0) return Visibility.Visible;
            else return Visibility.Collapsed;
        }
        else return Visibility.Collapsed;
        // if (user != null &&  user.Projects.Count > 0 && user.JobTitle == JobTitleName.ProjectManager)
        // {
        //     Console.Out.WriteLine($"{user} {user.JobTitle}");
        //     return Visibility.Visible;
        // }
        // else if (user != null && user.Projects.Count > 0 && user.JobTitle == JobTitleName.TeamLeader && parameter is string view && view.Equals("TaskView"))
        // {
        //     Console.Out.WriteLine($"{user} {user.JobTitle}");
        //     return Visibility.Visible;
        // }
        // else
        // {
        //     return Visibility.Collapsed;
        // }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}