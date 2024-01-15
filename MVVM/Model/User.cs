using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;

namespace ScrumApp.MVVM.Model;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public List<Project> Projects { get; set; } = new();
    public List<Task> Tasks { get; set; }

    public JobTitleName JobTitle { get; set; }
    
    public User(string firstName, string lastName, string email, string password, JobTitleName jobTitle)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        JobTitle = jobTitle;
        Projects = new List<Project>();
        Tasks = new List<Task>();
    }
    
    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}