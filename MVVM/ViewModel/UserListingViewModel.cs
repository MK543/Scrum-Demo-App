using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MVVMEssentials.ViewModels;
using NavigationTutorial.Commands;
using ScrumApp.MVVM.Model;

namespace NavigationTutorial.MVVM.ViewModel;

public class UserListingViewModel : ViewModelBase
{
    private readonly ObservableCollection<User> _userViewModels;

    public IEnumerable<User> UserViewModels => _userViewModels;

    private User _incomingUserViewModel;
    public User IncomingUserViewModel
    {
        get
        {
            return _incomingUserViewModel;
        }
        set
        {
            _incomingUserViewModel = value;
            OnPropertyChanged(nameof(IncomingUserViewModel));
        }
    }

    private User _removedUserViewModel;
    public User RemovedUserViewModel
    {
        get
        {
            return _removedUserViewModel;
        }
        set
        {
            _removedUserViewModel = value;
            OnPropertyChanged(nameof(RemovedUserViewModel));
        }
    }

    private User _insertedUserViewModel;
    public User InsertedUserViewModel
    {
        get
        {
            return _insertedUserViewModel;
        }
        set
        {
            _insertedUserViewModel = value;
            OnPropertyChanged(nameof(InsertedUserViewModel));
        }
    }

    private User _targetUserViewModel;
    public User TargetUserViewModel
    {
        get
        {
            return _targetUserViewModel;
        }
        set
        {
            _targetUserViewModel = value;
            OnPropertyChanged(nameof(TargetUserViewModel));
        }
    }

    public ICommand UserReceivedCommand { get; }
    public ICommand UserRemovedCommand { get; }
    public ICommand UserInsertedCommand { get; }

    public UserListingViewModel()
    {
        _userViewModels = new ObservableCollection<User>();

        UserReceivedCommand = new UserReceivedCommand(this);
        UserRemovedCommand = new UserRemovedCommand(this);
        UserInsertedCommand = new UserInsertedCommand(this);
    }

    //dodaje obiekt na liste jezeli go tam nie ma
    public void AddUser(User item)
    {
        if(!_userViewModels.Contains(item))
        {
            _userViewModels.Add(item);
        }
    }


    public void InsertUser(User insertedUser, User targetUser)
    {
        if(insertedUser == targetUser)
        {
            return;
        }

        int oldIndex = _userViewModels.IndexOf(insertedUser);
        int nextIndex = _userViewModels.IndexOf(targetUser);

        if(oldIndex != -1 && nextIndex != -1)
        {
            _userViewModels.Move(oldIndex, nextIndex);
        }
    }

    public void RemoveUser(User item)
    {
        _userViewModels.Remove(item);
    }
}