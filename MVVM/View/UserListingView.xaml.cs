using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NavigationTutorial.MVVM.View;

public partial class UserListingView : UserControl
{
public static readonly DependencyProperty IncomingUserProperty =
        DependencyProperty.Register("IncomingUser", typeof(object), typeof(UserListingView), 
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    
    public object IncomingUser
    {
        get { return (object)GetValue(IncomingUserProperty); }
        set { SetValue(IncomingUserProperty, value); }
    }
    
    public static readonly DependencyProperty RemovedUserProperty =
        DependencyProperty.Register("RemovedUser", typeof(object), typeof(UserListingView),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    
    public object RemovedUser
    {
        get { return (object)GetValue(RemovedUserProperty); }
        set { SetValue(RemovedUserProperty, value); }
    }
    
    public static readonly DependencyProperty UserDropCommandProperty =
        DependencyProperty.Register("UserDropCommand", typeof(ICommand), typeof(UserListingView), 
            new PropertyMetadata(null));
    
    public ICommand UserDropCommand
    {
        get { return (ICommand)GetValue(UserDropCommandProperty); }
        set { SetValue(UserDropCommandProperty, value); }
    }
    
    public static readonly DependencyProperty UserRemovedCommandProperty =
        DependencyProperty.Register("UserRemovedCommand", typeof(ICommand), typeof(UserListingView), 
            new PropertyMetadata(null));

    public ICommand UserRemovedCommand
    {
        get { return (ICommand)GetValue(UserRemovedCommandProperty); }
        set { SetValue(UserRemovedCommandProperty, value); }
    }
    
    public static readonly DependencyProperty UserInsertedCommandProperty =
        DependencyProperty.Register("UserInsertedCommand", typeof(ICommand), typeof(UserListingView), 
            new PropertyMetadata(null));

    public ICommand UserInsertedCommand
    {
        get { return (ICommand)GetValue(UserInsertedCommandProperty); }
        set { SetValue(UserInsertedCommandProperty, value); }
    }
    
    public static readonly DependencyProperty InsertedUserProperty =
        DependencyProperty.Register("InsertedUser", typeof(object), typeof(UserListingView),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public object InsertedUser
    {
        get { return (object)GetValue(InsertedUserProperty); }
        set { SetValue(InsertedUserProperty, value); }
    }
    
    public static readonly DependencyProperty TargetUserProperty =
        DependencyProperty.Register("TargetUser", typeof(object), typeof(UserListingView), 
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public object TargetUser
    {
        get { return (object)GetValue(TargetUserProperty); }
        set { SetValue(TargetUserProperty, value); }
    }

    public UserListingView()
    {
        InitializeComponent();
    }
    
    private void User_MouseMove(object sender, MouseEventArgs e)
    {
        if(e.LeftButton == MouseButtonState.Pressed &&
           sender is FrameworkElement frameworkElement)
        {
            object user = frameworkElement.DataContext;

            DragDropEffects dragDropResult = DragDrop.DoDragDrop(frameworkElement, 
                new DataObject(DataFormats.Serializable, user), 
                DragDropEffects.Move);

            if(dragDropResult == DragDropEffects.None)
            {
                AddUser(user);
            }
        }
    }
    
    private void User_DragOver(object sender, DragEventArgs e)
    {
        if (UserInsertedCommand?.CanExecute(null) ?? false)
        {
            if(sender is FrameworkElement element)
            {
                TargetUser = element.DataContext;
                InsertedUser = e.Data.GetData(DataFormats.Serializable);

                UserInsertedCommand?.Execute(null);
            }
        }
    }
    
    private void UserList_DragOver(object sender, DragEventArgs e)
    {
        object user = e.Data.GetData(DataFormats.Serializable);
        AddUser(user);
    }
    
    private void AddUser(object user)
    {
        if (UserDropCommand?.CanExecute(null) ?? false)
        {
            IncomingUser = user;
            UserDropCommand?.Execute(null);
        }
    }
    
    private void UserList_DragLeave(object sender, DragEventArgs e)
    {
        HitTestResult result = VisualTreeHelper.HitTest(lvItems, e.GetPosition(lvItems));

        if(result == null)
        {
            if (UserRemovedCommand?.CanExecute(null) ?? false)
            {
                RemovedUser = e.Data.GetData(DataFormats.Serializable);
                UserRemovedCommand?.Execute(null);
            }
        }    
    }
    /*
    private void User_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is ListViewItem item && item.DataContext is User selectedUser)
        {
            string userInfo = $"Nazwa: {selectedUser.FirstName}\nOpis: {selectedUser.LastName}\n";

            MessageBox.Show(userInfo, "Szczegóły uzytkownika", MessageBoxButton.OK);
        }
    }*/
}