using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.ViewModel.ManageUsersOperationClass;

public class UpdateProjectUser : ObservableObject
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
    
    private User _selectedUser;
    
    public User SelectedUser
    {
        get => _selectedUser;
        set
        {
            _selectedUser = value;
            OnPropertyChanged(nameof(SelectedUser));
            FirstName = _selectedUser.FirstName;
            LastName = _selectedUser.LastName;
            Email = _selectedUser.Email;
        }
    }
    
    private ObservableCollection<User> _allUsers;
    
    public ObservableCollection<User> AllUsers
    {
        get => _allUsers;
    }
    
    public ObservableCollection<User> Users { get; set; }

    private string _invalidUserSelectLabel;
    private string _invalidUserFirstNameLabel;
    private string _invalidUserLastNameLabel;
    private string _invalidEmailLabel;
    private string _invalidPasswordLabel;

    public string InvalidUserSelectLabel
    {
        get => _invalidUserSelectLabel;
        set
        {
            _invalidUserSelectLabel = value;
            OnPropertyChanged();
        }
    }
    
    public string InvalidUserFirstNameLabel
    {
        get => _invalidUserFirstNameLabel;
        set
        {
            _invalidUserFirstNameLabel = value;
            OnPropertyChanged();
        }
    }
    
    public string InvalidUserLastNameLabel
    {
        get => _invalidUserLastNameLabel;
        set
        {
            _invalidUserLastNameLabel = value;
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
    
    
    
    public UpdateProjectUser()
    {
        LoadUsers();
    }
    
    public void LoadUsers()
    {
        using (var dbContext = new ScrumDbContext())
        {
            var usersUpdate = dbContext.Users.Where(u => u.JobTitle == JobTitleName.TeamMember).ToList();
            var loggedUser = ((App)Application.Current).LoggedUser;
            usersUpdate.Insert(0, loggedUser);
            _allUsers = new ObservableCollection<User>(usersUpdate);
            Users = new ObservableCollection<User>(_allUsers);
        }
    }

    private bool Validate()
    {
        var isFnameValid = true;
        var isLnameValid = true;
        var isEmailValid = true;
        var isPasswordValid = true;
        var isUserSelected = true;
        
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

        isUserSelected = SelectedUser != null;

        if (isFnameValid && isLnameValid && isEmailValid && isPasswordValid && isUserSelected)
        {
            return true;
        }
        else
        {
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
                InvalidUserFirstNameLabel = "Invalid first name";
            }
            if (!isLnameValid)
            {
                System.Diagnostics.Debug.WriteLine("Nie przeszlo walidacji nazwiska");
                System.Diagnostics.Debug.WriteLine(LastName);
                InvalidUserLastNameLabel = "Invalid last name";
            }
            if (!isEmailValid)
            {
                System.Diagnostics.Debug.WriteLine("Nie przeszlo walidacji emaila");
                System.Diagnostics.Debug.WriteLine(Email);
                InvalidEmailLabel = "Email format:\nexample@example.com";
            }

            if (!isUserSelected)
            {
                InvalidUserSelectLabel = "User must be selected";
            }
            return false;
        }
        
    }
    
    public User UpdateUser()
    {
        if (Validate())
        {
            User userToUpdate = SelectedUser;
            userToUpdate.FirstName = FirstName;
            userToUpdate.LastName = LastName;
            userToUpdate.Email = Email;
            userToUpdate.Password = Password;
            return userToUpdate;
        }
        return null;
    }
    
}