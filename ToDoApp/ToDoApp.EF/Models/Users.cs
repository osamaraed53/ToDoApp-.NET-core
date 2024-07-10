namespace ToDoApp.EF.Models;


public class User
{
    public int Id { get; set; } 

    public  string? Username { get; set; }

    public required string Email { get; set; }

    public required string HashPassword { get; set; }

    public DateTime Created_at { get; set; } = DateTime.Now;


    public ICollection<UserTask> UserTasks { get; set; }


}
