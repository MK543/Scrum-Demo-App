using System.Windows;
using System.Windows.Controls;
using NavigationTutorial.MVVM.ViewModel;
using NavigationTutorial.MVVM.ViewModel.ManageUsersOperationClass;

namespace NavigationTutorial.MVVM.View;

public partial class ManageUsersView : UserControl
{
    public ManageUsersView()
    {
        InitializeComponent();
    }
    private void RegisterViewPasswordBoxAddUser_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is ManageUsersViewModel manageUsersViewModel)
            manageUsersViewModel.CreateProjectUser.Password = ((PasswordBox)sender).Password;
    }
    private void RegisterViewPasswordBoxUpdateUser_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is ManageUsersViewModel manageUsersViewModel)
            manageUsersViewModel.UpdateProjectUser.Password = ((PasswordBox)sender).Password;
    }
}