using Microsoft.AspNetCore.Http;
using ToDoApp.Core.DTOs;
using ToDoApp.EF.Models;

namespace ToDoApp.Core.Interfaces;

public interface IUserTaskRepository : IBaseRepository<UserTask>
{

    string GetTaskById(HttpRequest httpRequest, int id);
    IEnumerable<TaskDTO> GetUserTasks(HttpRequest httpRequest);
    void CreateTask(HttpRequest httpRequest, CreateTaskDTO request);
    TaskDTO UpdateTask(HttpRequest httpRequest, UpdateTaskDTO request);
    TaskDTO UpdateTaskState(HttpRequest httpRequest, PutTaskStatusDTO request);
    void DeleteTask(HttpRequest httpRequest, int taskId);

}
