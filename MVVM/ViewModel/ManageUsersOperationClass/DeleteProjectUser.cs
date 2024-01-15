using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.ViewModel.ManageUsersOperationClass;

public class DeleteProjectUser : Core.ViewModel
{
    private User _selectedUser;
    
    public User SelectedUser
    {
        get => _selectedUser;
        set
        {
            _selectedUser = value;
            OnPropertyChanged(nameof(SelectedUser));
        }
    }
    
    private string _searchUser;
    public string SearchUser
    {
        get => _searchUser;
        set
        {
            _searchUser = value;
            OnPropertyChanged(nameof(SearchUser));
        }
    }
    
    private ObservableCollection<User> _allUsers;
    public ObservableCollection<User> AllUsers
    {
        get => _allUsers;
    }
    
    
    public void LoadUsers()
    {
        using (var dbContext = new ScrumDbContext())
        {
            var teamMembersUsers = dbContext.Users.Where(u => u.JobTitle == JobTitleName.TeamMember).ToList();
            _allUsers = new ObservableCollection<User>(teamMembersUsers);
            Users = new ObservableCollection<User>(_allUsers);
        }
    }
    
    public ObservableCollection<User> Users { get; set; }

    private string _invalidUserSelectLabel;

    public string InvalidUserSelectLabel
    {
        get => _invalidUserSelectLabel;
        set
        {
            _invalidUserSelectLabel = value;
            OnPropertyChanged();
        }
    }

    
    public DeleteProjectUser()
    {
        LoadUsers();
        // PropertyChanged += UpdateFilteredUsers;
    }
    
    // private void UpdateFilteredUsers(object? sender, PropertyChangedEventArgs e)
    // {
    //     if (e.PropertyName == "SearchUser" && SearchUser != null)
    //     {
    //         var filteredUsers = AllUsers
    //             .Where(user =>
    //                 user.FirstName.Contains(SearchUser, StringComparison.OrdinalIgnoreCase))
    //             .ToList();
    //         Users = new ObservableCollection<User>(filteredUsers);
    //         OnPropertyChanged(nameof(Users));
    //     }
    //    
    // }
    
    private bool Validate()
    {
        InvalidUserSelectLabel = SelectedUser != null ? null : "User must be selected";
        return SelectedUser != null;
    }
    public User RemoveUser()
    {
        if (Validate())
        {
            System.Diagnostics.Debug.WriteLine("Przeszlo walidacja");
            return SelectedUser;
        }
        System.Diagnostics.Debug.WriteLine("Nie przechodzi walidacji");
        System.Diagnostics.Debug.WriteLine("Selected User:");
        return null;
    }
    
}