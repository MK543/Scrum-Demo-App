using System;
using System.Linq;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.ViewModel.ManageUsersOperationClass;

public class CreateProjectUser : ObservableObject
{
    private string _firstName;

    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            OnPropertyChanged(nameof(FirstName));
        }
    }
    
    private string _lastName;
    
    public string LastName
    {
        get => _lastName;
        set
        {
            _lastName = value;
            OnPropertyChanged(nameof(LastName));
        }
    }
    
    private string _email;

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged(nameof(Email));
        }
    }
    
    private string _password;

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    private string _invalidFirstNameLabel;
    private string _invalidLastNameLabel;
    private string _invalidEmailLabel;
    private string _invalidPasswordLabel;

    public string InvalidFirstNameLabel
    {
        get => _invalidFirstNameLabel;
        set
        {
            _invalidFirstNameLabel = value;
            OnPropertyChanged();
        }
    }
    
    public string InvalidLastNameLabel
    {
        get => _invalidLastNameLabel;
        set
        {
            _invalidLastNameLabel = value;
            OnPropertyChanged();
        }
    }
    
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
    

    private bool Validate()
    {

        InvalidFirstNameLabel = null;
        InvalidLastNameLabel = null;
        InvalidEmailLabel = null;
        InvalidPasswordLabel = null;
        
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
        if (_email is null || !regex.IsMatch(_email)) isEmailValid = false;
        
        string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
        regex = new Regex(passwordPattern);
        if (_password is null || !regex.IsMatch(_password)) isPasswordValid = false;
        
        Console.Out.WriteLine($"{isFnameValid} {isLnameValid} {isEmailValid} {isPasswordValid}");

        if (isFnameValid && isLnameValid && isEmailValid && isPasswordValid)
        {
            var dbContext = new ScrumDbContext();
            bool isEmailAlreadyTaken = dbContext.Users.Any(u => u.Email == _email);
            if (isEmailAlreadyTaken)
            {
                InvalidEmailLabel = "Email is already taken";
                return false;
            }
            else
            {
                return true;
            }
        }

        if (!isPasswordValid)
        {
            System.Diagnostics.Debug.WriteLine("Nie przeszlo walidacji hasla");
            System.Diagnostics.Debug.WriteLine(Password);
            InvalidPasswordLabel = "Password is too weak";
        }
        if (!isFnameValid)
        {
            System.Diagnostics.Debug.WriteLine("Nie przeszlo walidacji imienia");
            System.Diagnostics.Debug.WriteLine(FirstName);
            InvalidEmailLabel = "Email format:\nexample@example.com";
        }
        if (!isLnameValid)
        {
            System.Diagnostics.Debug.WriteLine("Nie przeszlo walidacji nazwiska");
            System.Diagnostics.Debug.WriteLine(LastName);
            InvalidLastNameLabel = "Invalid last name";
        }
        if (!isEmailValid)
        {
            System.Diagnostics.Debug.WriteLine("Nie przeszlo walidacji emaila");
            System.Diagnostics.Debug.WriteLine(Email);
            if (!isFnameValid) InvalidFirstNameLabel = "Invalid first name";
        }
        
        return false;
    }
    
    public User CreateUser()
    {
        if (Validate())
        {
            System.Diagnostics.Debug.WriteLine("Przeszlo walidacje");
            User newUser = new User(FirstName, LastName, Email, Password, JobTitleName.TeamMember);
            return newUser;
        }
        System.Diagnostics.Debug.WriteLine("Nie przeszlo walidacji");
        return null;
    }

    public void Refresh()
    {
        FirstName = "";
        LastName = "";
        Email = "";
        Password = null;
        _password = null;
    }
}


