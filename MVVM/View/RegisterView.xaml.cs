using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using NavigationTutorial.MVVM.ViewModel;

namespace NavigationTutorial.MVVM.View;

public partial class RegisterView : UserControl
{
    public RegisterView()
    {
        InitializeComponent();
           
    }
    
    private void RegisterViewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is RegisterViewModel registerViewModel)
            registerViewModel.Password = ((PasswordBox)sender).Password;
    }
    
}