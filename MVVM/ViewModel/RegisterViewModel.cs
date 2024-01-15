using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.ViewModel;

public class RegisterViewModel : Core.ViewModel
{
    //Register Form fields
    private string? _email;
    private string? _password;
    private string? _fName;
    private string? _lName;
    
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

    public string FirstName
    {
        get => _fName;
        set
        {
            _fName = value;
            OnPropertyChanged(nameof(FirstName));
        }
    }
    
    public string LastName
    {
        get => _lName;
        set
        {
            _lName = value;
            OnPropertyChanged(nameof(LastName));
        }
    }
    
    //Error Labels
    private string? _invalidFirstNameLabel;
    private string? _invalidLastNameLabel;
    private string? _invalidEmailLabel;
    private string? _invalidPasswordLabel;

    public String InvalidFirstNameLabel
    {
        get => _invalidFirstNameLabel;
        set
        {
            _invalidFirstNameLabel = value;
            OnPropertyChanged();
        }
    }
    
    public String InvalidLastNameLabel
    {
        get => _invalidLastNameLabel;
        set
        {
            _invalidLastNameLabel = value;
            OnPropertyChanged();
        }
    }
    
    public String InvalidEmailLabel
    {
        get => _invalidEmailLabel;
        set
        {
            _invalidEmailLabel = value;
            OnPropertyChanged();
        }
    }
    
    public String InvalidPasswordLabel
    {
        get => _invalidPasswordLabel;
        set
        {
            _invalidPasswordLabel = value;
            OnPropertyChanged();
        }
    }
    
    public ICommand RegisterCommand { get; }
    
    public RegisterViewModel()
    {
        RegisterCommand = new RelayCommand(Register);
    }

    public void ClearErrorFields()
    {
        InvalidFirstNameLabel = null;
        InvalidLastNameLabel = null;
        InvalidEmailLabel = null;
        InvalidPasswordLabel = null;
    }
    
    private void Register()
    {
        ClearErrorFields();
        var isFnameValid = true;
        var isLnameValid = true;
        var isEmailValid = true;
        var isPasswordValid = true;
        
        // In app fields validation
        string namePattern = @"^[\p{L} \.'\-]+$";
        Regex regex = new Regex(namePattern);
        if (FirstName == null || FirstName.Contains(" ") || !regex.IsMatch(FirstName)) isFnameValid = false;
        if (LastName == null || LastName.Contains(" ") || !regex.IsMatch(LastName)) isLnameValid = false;
        string emailPattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
        regex = new Regex(emailPattern);
        if (Email is null || !regex.IsMatch(Email)) isEmailValid = false;
        string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
        regex = new Regex(passwordPattern);
        if (Password is null || !regex.IsMatch(Password)) isPasswordValid = false;
        Console.Out.WriteLine($"{isFnameValid} {isLnameValid} {isEmailValid} {isPasswordValid}");
        
        if (isFnameValid && isLnameValid && isEmailValid && isPasswordValid)
        {
            var dbContext = new ScrumDbContext();
            var newUser = new User(FirstName, LastName, Email, Password, JobTitleName.TeamMember);
            bool isEmailAlreadyTaken = dbContext.Users.Any(u => u.Email == newUser.Email);
        
            if (isEmailAlreadyTaken)
            {
                InvalidEmailLabel = "Email already exists";
            }
            else
            {
                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();
                Email = null;
                Password = null;
                FirstName = null;
                LastName = null;
                Console.WriteLine("Success");
            }
            
            
        }
        else
        {
            if (!isFnameValid)  InvalidFirstNameLabel = "Invalid first name";
            if (!isLnameValid) InvalidLastNameLabel = "Invalid last name";
            if (!isEmailValid) InvalidEmailLabel = "Email format example@example.com";
            if (!isPasswordValid) InvalidPasswordLabel = "Password is too weak";
        }
        
    }
}