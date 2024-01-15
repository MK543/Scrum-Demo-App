using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.ViewModel;

public class HomeViewModel : Core.ViewModel
{
    private string _email;
    private string _password;
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged(nameof(Email));
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }
    
    private string _invalidEmailLabel;
    private string _invalidPasswordLabel;

    public string InvalidEmailLabel
    {
        get => _invalidEmailLabel;
        set
        {
            _invalidEmailLabel = value;
            OnPropertyChanged();
        }
    }
    
    public string InvalidPasswordLabel
    {
        get => _invalidPasswordLabel;
        set
        {
            _invalidPasswordLabel = value;
            OnPropertyChanged();
        }
    }
   
    
    public ICommand LoginCommand { get; }
    
    public HomeViewModel()
    {
        LoginCommand = new RelayCommand(Login);
    }
    
    public void ClearErrorFields()
    {
        InvalidEmailLabel = null;
        InvalidPasswordLabel = null;
    }

    private void Login()
    {
        // Kod obsługi logowania
        Console.WriteLine("Login clicked");
        
        ClearErrorFields();
        var isEmailValid = true;
        var isPasswordValid = true;
        
        // In app fields validation
        string emailPattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
        Regex regex = new Regex(emailPattern);
        Console.Out.WriteLine(_email);
        Console.Out.WriteLine(_password);
        if (_email is null || !regex.IsMatch(_email)) isEmailValid = false;
        string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
        regex = new Regex(passwordPattern);
        if (_password is null || !regex.IsMatch(_password)) isPasswordValid = false;
        Console.Out.WriteLine($"{isEmailValid} {isPasswordValid}");
       
        
        if (isEmailValid && isPasswordValid)
        {
            var dbContext = new ScrumDbContext();
            bool isUserExist = dbContext.Users.Any(u => u.Email == _email && u.Password == _password);

            if (isUserExist)
            {
                Console.Out.WriteLine("Login in");
                var loggedInUser = dbContext.Users.FirstOrDefault(u => u.Email == _email && u.Password == _password);
                int userId = loggedInUser.Id;
                ((App)Application.Current).ChangeToProjectWindow(userId);
                Email = null;
                Password = null;
                
            }
            else
            {
                InvalidEmailLabel = "Invalid Email or Password";
            }
        }
        else
        {
            if (!isEmailValid) InvalidEmailLabel = "Email does not fit criteria";
            if (!isPasswordValid) InvalidPasswordLabel = "Password does not fit criteria";
        }

    }
    
}