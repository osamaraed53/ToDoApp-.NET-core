using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ToDoApp.Data;
using ToDoApp.Helpers;
using System.Text.Json;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoApp.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class UserTaskController(IConfiguration config, ToDoAppDataContext context) : ControllerBase
{
    private readonly IConfiguration _config = config;

    private readonly ToDoAppDataContext _context = context;


    // GET: api/<ValuesController>
    [HttpGet(Name = "GetUserTasks")]
    public IEnumerable<User_Task> GetTasks()
    {
        var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
        var payload = JwtSecurityTokenHandlerWrapper.ValidateJwtToken(_config, token);
        int user_id = Int16.Parse(payload["user_id"]);

        return _context.Tasks.Where(s => s.User_id == user_id);
    }

    //GET api/<ValuesController>/5
    [HttpGet("{task_id}", Name = "GetTaskById")]
    public IActionResult Get(int task_id)
    {
        try
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var payload = JwtSecurityTokenHandlerWrapper.ValidateJwtToken(_config, token);
            int user_id = Int16.Parse(payload["user_id"]);

            User_Task result = _context.Tasks.FirstOrDefault(s => s.Task_id == task_id);

            if (result == null) return NotFound();
            if (result.User_id != user_id) return StatusCode(401, "Unauthorized");

            var output = JsonSerializer.Serialize(result);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while retrieving the task.");
        }
    }



    //POST api/<ValuesController>

    public struct CreateTask
    {
        public string Task_description { get; set; }
    }

    [HttpPost(Name = "CreateTaskForUser")]
    public async Task<string> Post([FromBody] CreateTask value)
    {
        User_Task user_task;

        try
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var payload = JwtSecurityTokenHandlerWrapper.ValidateJwtToken(_config, token);
            int user_id = Int16.Parse(payload["user_id"]);


            user_task = new User_Task() { User_id = user_id, Task_description = value.Task_description };
            _context.Tasks.Add(user_task);
            await _context.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return "done";
    }

    public struct PutTask_description

    {
        public string Task_description { get; set; }
        public int Task_id { get; set; }
    }


    //PUT api/<ValuesController>/5
    [HttpPut("UpdateTaskDescription", Name = "PutTaskDescription")]
    public IActionResult Put([FromBody] PutTask_description value)
    {
        try
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var payload = JwtSecurityTokenHandlerWrapper.ValidateJwtToken(_config, token);
            int user_id = Int16.Parse(payload["user_id"]);

            var dbTask = _context.Tasks.SingleOrDefault(s => s.Task_id == value.Task_id);

            if (dbTask == null) return StatusCode(404, "Task Not Found");

            if (dbTask.User_id != user_id) return StatusCode(401, "Unauthorized");

            dbTask.Task_description = value.Task_description;

            _context.SaveChanges();

        }
        catch
        {
            return StatusCode(500, "Internal Server Error");
        }

        return StatusCode(201, "Task Description Updated");
    }








    public struct PutTaskStatus

    {
        public string Status { get; set; }
        public int Task_id { get; set; }
    }
    public enum TaskStatus
    {
        Pending,
        Completed,
        InProgress,
        Cancelled,
    }

    //PUT api/<ValuesController>/5
    [HttpPut("UpdateTaskStatus", Name = "UpdateTaskStatus")]
    public IActionResult UpdateTaskStatus([FromBody] PutTaskStatus value)
    {
        try
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var payload = JwtSecurityTokenHandlerWrapper.ValidateJwtToken(_config, token);
            int user_id = Int16.Parse(payload["user_id"]);

            var dbTask = _context.Tasks.SingleOrDefault(s => s.Task_id == value.Task_id);

            if (dbTask == null) StatusCode(404, "Task Not Found");
            if (dbTask?.User_id != user_id) return StatusCode(401, "Unauthorized");


            if (System.Enum.IsDefined(typeof(string), value.Status))
            {
                dbTask.Status = value.Status;
                _context.SaveChanges();
                return StatusCode(204, "Updated done");
            }
            else
            {
                return StatusCode(405, "Not allowed");
            }
        }
        catch
        {
            return StatusCode(500, "Internal Server Error");
        }


    }





    // DELETE api/<ValuesController>/5
    [HttpDelete("{Task_id}")]
    public IActionResult Delete(int Task_id)
    {
        try
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var payload = JwtSecurityTokenHandlerWrapper.ValidateJwtToken(_config, token);
            int user_id = Int16.Parse(payload["user_id"]);

            var dbTask = _context.Tasks.SingleOrDefault(s => s.Task_id == Task_id);

            if (dbTask == null) StatusCode(404, "Task Not Found");
            if (dbTask.User_id != user_id) return StatusCode(401, "Unauthorized");



            // Assign the valid status to the task
            _context.Tasks.Remove(dbTask);
            _context.SaveChanges();

            return Ok("Deleted done");


        }
        catch
        {

            return StatusCode(500, "Internal Server Error");
        }

    }



}

