using System.Threading.Tasks;
using ToDoApp.EF.Models;

namespace ToDoApp.Core.DTOs.DTOsMapper;

public static class UserTasksMapper
{

    public static TaskDTO? ToTaskDTO(this UserTask userTask)
    {

        return new TaskDTO
        {
            Task_id = userTask.Id,
            Title = userTask.Title!,
            TaskDescription = userTask.TaskDescription,
            TaskStatus = userTask.Status,

        };
    }

    public static IEnumerable<TaskDTO> ToTaskDTO(this IEnumerable<UserTask> userTasks)
    {
        return from task in userTasks
           select new TaskDTO
           {
               Task_id = task.Id,
               Title = task.Title!,
               TaskDescription = task.TaskDescription,
               TaskStatus = task.Status,
           };
    }


}
