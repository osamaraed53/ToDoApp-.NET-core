using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using ToDoApp.Core.Constant;
using ToDoApp.Core.CustomExceptions;
using ToDoApp.Core.DTOs;
using ToDoApp.Core.DTOs.DTOsMapper;
using ToDoApp.Core.ExtensionMethods;
using ToDoApp.Core.Interfaces;
using ToDoApp.EF.Data;
using ToDoApp.EF.Models;

namespace ToDoApp.Core.Repositories;

public class UserTaskRepository
    (IConfiguration configuration, AppDbContext context, JwtSecurityTokenHandler jwtSecurityTokenHandler)
    : BaseRepository<UserTask>(configuration, context, jwtSecurityTokenHandler), IUserTaskRepository
{
    private readonly IConfiguration _configuration = configuration;
    private readonly AppDbContext _context = context;

    public string? GetTaskById(HttpRequest httpRequest, int id)
    {
        int userId = GetUserId(httpRequest);
        UserTask result = GetById(id);

        if (result == null) throw new NotFoundException(ExceptionMessages.NOT_EXIST_ERROR);
        if (result.UserId != userId) throw new ForbiddenException(ExceptionMessages.UNAUTHORIZED_ERROR);

        return result.ToTaskDTO()?.Serialize();

    }

    public IEnumerable<TaskDTO> GetUserTasks(HttpRequest httpRequest)
    {
        int user_id = GetUserId(httpRequest);
        return FindAll(s => s.UserId == user_id).ToTaskDTO();
    }

    public void CreateTask(HttpRequest httpRequest, CreateTaskDTO request)
    {

        UserTask user_task;
        try
        {
            int userId = GetUserId(httpRequest);
            user_task = new UserTask() { UserId = userId, Title = request.Title, TaskDescription = request.TaskDescription };
            Add(user_task);
        }
        catch
        {
            throw new Exception(ExceptionMessages.INTERNAL_SERVER_ERROR);
        }
    }

    public TaskDTO UpdateTask(HttpRequest httpRequest, UpdateTaskDTO request)
    {

        int userId = GetUserId(httpRequest);

        UserTask userTask = GetById(request.Task_id);

        if (userTask == null)
            throw new NotFoundException(ExceptionMessages.NOT_EXIST_ERROR);

        if (userTask.UserId != userId)
            throw new ForbiddenException(ExceptionMessages.UNAUTHORIZED_ERROR);

        if (string.IsNullOrEmpty(request.TaskDescription))
            userTask.TaskDescription = request.TaskDescription;

        if (string.IsNullOrEmpty(request.Title)) userTask.Title = request.Title;


        return userTask.ToTaskDTO()!;

    }


    public TaskDTO UpdateTaskState(HttpRequest httpRequest, PutTaskStatusDTO request)
    {

        int userId = GetUserId(httpRequest);

        var userTask = GetById(request.TaskId);

        if (userTask == null)
            throw new NotFoundException(ExceptionMessages.NOT_EXIST_ERROR);

        if (userTask.UserId != userId)
            throw new ForbiddenException(ExceptionMessages.UNAUTHORIZED_ERROR);


        if (string.IsNullOrEmpty(request.Status) && Enum.IsDefined(typeof(EF.Models.TaskStatus), request.Status!))
        {
            userTask.Status = request.Status!;
        }
        else
        {
            throw new NotAllowedException(ExceptionMessages.NOT_ALLOWED);
        }

        return userTask.ToTaskDTO()!;

    }


    public void DeleteTask(HttpRequest httpRequest, int taskId)
    {

        int userId = GetUserId(httpRequest);

        var dbTask = GetById(taskId);

        if (dbTask?.UserId != userId) throw new ForbiddenException(ExceptionMessages.UNAUTHORIZED_ERROR);
        if (dbTask == null) throw new NotFoundException("Task Not Found");

        Delete(dbTask);

    }

}
