namespace ToDoApp.Core.DTOs;

public record class PutTaskStatusDTO
{
    public required int TaskId { get; set; }
    public required string Status { get; set; }
}

public record class TaskDTO
{
    public required int Task_id { get; set; }
    public required string Title { get; set; }
    public string? TaskDescription { get; set; }

    public string? TaskStatus { get; set;}
}


public record class UpdateTaskDTO
{
    public required int Task_id { get; set; }
    public required string Title { get; set; }
    public string? TaskDescription { get; set; }
    public string? TaskStatus { get; set; }
}

public record class CreateTaskDTO
{
    public required string Title { get; set; }
    public string? TaskDescription { get; set; }
}


