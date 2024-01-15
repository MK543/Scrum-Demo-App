using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using NavigationTutorial.MVVM.ViewModel;

namespace NavigationTutorial.MVVM.View;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
        if (this.DataContext is HomeViewModel homeViewModel)
            homeViewModel.PropertyChanged += ShowValidationErrors;
    }
    
    private void LoginViewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is HomeViewModel loginViewModel)
        {
            loginViewModel.Password = ((PasswordBox)sender).Password;
        }
    }
    
    public void ShowValidationErrors(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName.Contains("EmailExistError"))
            InvalidEmailLabel.Content = "Wrong email or password";
        else InvalidEmailLabel.Content = null;
        
        if (e.PropertyName.Contains("EmailFormatError"))
            InvalidEmailLabel.Content = "Email must be in format: \r\n example@example.com";
        else InvalidEmailLabel.Content = null;
        
        if (e.PropertyName.Contains("PasswordError"))
            InvalidPasswordLabel.Content = "Wrong password";
        else InvalidPasswordLabel.Content = null;
    }
}