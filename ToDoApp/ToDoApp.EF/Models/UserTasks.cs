namespace ToDoApp.EF.Models;


public enum TaskStatus
{
    Pending,
    Completed,
    InProgress,
    Cancelled,
}
public class UserTask
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public  User User { get; set; }

    public  string? Title { get; set; }

    public string? TaskDescription { get; set; }

    public DateTime? DueDate { get; set; } = DateTime.Now;

    public string Status { get; set; } = TaskStatus.Pending.ToString();
    public string Created_at { get; set; } = (DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss");
}