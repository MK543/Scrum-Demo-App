using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NavigationTutorial.Core;
using NavigationTutorial.MVVM.View;
using NavigationTutorial.MVVM.ViewModel;
using NavigationTutorial.MVVM.ViewModel.ManageProjectsOperationClass;
using NavigationTutorial.MVVM.ViewModel.ManageTasksOperationClass;
using NavigationTutorial.MVVM.ViewModel.ManageUsersOperationClass;
using NavigationTutorial.Services;
using ScrumApp.MVVM.Model;
using Task = System.Threading.Tasks.Task;

namespace NavigationTutorial
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        
        public ServiceProvider ServiceProvider
        {
            get => _serviceProvider;
        }

        public App()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });
            serviceCollection.AddTransient<ProjectsWindow>(provider => new ProjectsWindow()
            {
                DataContext = provider.GetRequiredService<ProjectsWindowViewModel>()
            });
            serviceCollection.AddTransient<HomeView>(provider => new HomeView()
            {
                DataContext = provider.GetRequiredService<HomeViewModel>()
            });
            serviceCollection.AddTransient<RegisterView>(provider => new RegisterView()
            {
                DataContext = provider.GetRequiredService<RegisterViewModel>()
            });
            serviceCollection.AddSingleton<AboutUsView>(provider => new AboutUsView()
            {
                DataContext = provider.GetRequiredService<AboutUsViewModel>()
            });
            //
            serviceCollection.AddTransient<TaskView>(provider => new TaskView()
            {
                DataContext = provider.GetRequiredService<TaskViewModel>()
            });
            serviceCollection.AddTransient<ProjectsView>(provider => new ProjectsView()
            {
                DataContext = provider.GetRequiredService<ProjectsViewModel>()
            });
            serviceCollection.AddTransient<MembersView>(provider => new MembersView()
            {
                DataContext = provider.GetRequiredService<MembersViewModel>()
            });
            // Zmiana
            serviceCollection.AddTransient<MVVM.View.ManageTasksView>(provider => new MVVM.View.ManageTasksView()
            {
                DataContext = provider.GetRequiredService<ManageTasksViewModel>()
            });
            serviceCollection.AddTransient<MVVM.View.ManageProjectsView>(provider => new MVVM.View.ManageProjectsView()
            {
                DataContext = provider.GetRequiredService<ManageProjectsViewModel>()
            });
            serviceCollection.AddTransient<MVVM.View.ManageUsersView>(provider => new MVVM.View.ManageUsersView()
            {
                DataContext = provider.GetRequiredService<ManageUsersViewModel>()
            });
            
            //serviceCollection.AddSingleton<MainViewModel>();
            //serviceCollection.AddSingleton<ProjectsWindowViewModel>();
            // To Log On and Out we need to have multiple instance
            serviceCollection.AddTransient<MainViewModel>();
            serviceCollection.AddTransient<ProjectsWindowViewModel>();
            //
            serviceCollection.AddTransient<HomeViewModel>();
            serviceCollection.AddTransient<RegisterViewModel>();
            serviceCollection.AddSingleton<AboutUsViewModel>();
            //serviceCollection.AddSingleton<TaskViewModel>();
            serviceCollection.AddTransient<TaskViewModel>();
            serviceCollection.AddTransient<ProjectsViewModel>();
            //serviceCollection.AddSingleton<ProjectsViewModel>();
            serviceCollection.AddTransient<MembersViewModel>();
            
            // Zmiana
            serviceCollection.AddTransient<ManageTasksViewModel>();
            
            serviceCollection.AddTransient<CreateProjectTask>();
            serviceCollection.AddTransient<DeleteProjectTask>();
            serviceCollection.AddTransient<UpdateProjectTask>();
            //
            serviceCollection.AddTransient<ManageProjectsViewModel>();
            
            serviceCollection.AddTransient<CreateProject>();
            serviceCollection.AddTransient<DeleteProject>();
            serviceCollection.AddTransient<UpdateProject>();
            
            serviceCollection.AddTransient<ManageUsersViewModel>();
            
            serviceCollection.AddTransient<CreateProjectUser>();
            serviceCollection.AddTransient<UpdateProjectUser>();
            serviceCollection.AddTransient<DeleteProjectUser>();
            
            serviceCollection.AddSingleton<INavigationService, NavigationService>();
            serviceCollection.AddSingleton<Func<Type, ViewModel>>(serviceProvider =>
                viewModelType => (ViewModel)serviceProvider.GetRequiredService(viewModelType));
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private MainWindow _mainWindow;
        private ProjectsWindow _projectWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            _mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            _mainWindow.Show();
            if (_mainWindow.DataContext is MainViewModel mainViewModel) mainViewModel.NavigateToHomeView.Execute(true);
            base.OnStartup(e);
        }

        public void ChangeToProjectWindow(int userId)
        {
            _mainWindow.Hide();
            _projectWindow = _serviceProvider.GetRequiredService<ProjectsWindow>();
            _projectWindow.Show();
            _mainWindow.Close();
            LoadUserData(userId);
            if (_projectWindow.DataContext is ProjectsWindowViewModel projectsWindowViewModel) 
                projectsWindowViewModel.NavigateToProjectsView.Execute(true);
        }
        
        public void ChangeToLoginWindow()
        {
            _projectWindow.Hide();
            _mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            _mainWindow.Show();
            
            if (_projectWindow != null)
            {
                _projectWindow.Close();
            }

            if (_mainWindow.DataContext is MainViewModel mainView) 
                mainView.NavigateToHomeView.Execute(true);
        }

        public void ExitApp()
        {
            _mainWindow.Close();
        }

        public User? LoggedUser { get; set; }

        public async Task UpdateUserData(int userId)
        {
            using (var dbContext = new ScrumDbContext())
            {
                LoggedUser = dbContext.Users
                    .Where(u => u.Id == userId)
                    .Include(u => u.Projects)
                    .ThenInclude(p => p.ProjectTasks)
                    .Include(u => u.Projects)
                    .ThenInclude(p => p.Users)
                    .FirstOrDefault();
                
                var project = dbContext.Projects.FirstOrDefault(p => p.Id == ProjectId);
                if (project != null) ProjectName = project.ProjectName;
            }
        }
        public async Task LoadUserData(int userId)
        {
            using (var dbContext = new ScrumDbContext())
            {
                LoggedUser = dbContext.Users
                    .Where(u => u.Id == userId)
                    .Include(u => u.Projects)
                        .ThenInclude(p => p.ProjectTasks)
                    .Include(u => u.Projects)
                        .ThenInclude(p => p.Users)
                    .FirstOrDefault();
                
                if (LoggedUser != null)
                {
                    // ustawienie domyslnej wartosci id projektu
                    ProjectId = LoggedUser.Projects.Select(p => p.Id).FirstOrDefault();
                    ProjectName = LoggedUser.Projects.Select(p => p.ProjectName).FirstOrDefault();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("User not found.");
                }
            }
        }

        private int _projectId;
        public int ProjectId
        {
            get => _projectId;
            set
            {
                if (_projectId != value)
                {
                    _projectId = value;
                    OnPropertyChanged(nameof(ProjectId));
                }
            }
        }
        
        private string _projectName;
        public string ProjectName
        {
            get => _projectName;
            set
            {
                if (_projectName != value)
                {
                    _projectName = value;
                    OnPropertyChanged(nameof(ProjectName));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}