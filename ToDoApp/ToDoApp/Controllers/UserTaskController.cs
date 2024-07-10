using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Core.Constant;
using ToDoApp.Core.DTOs;
using ToDoApp.Core.ExtensionMethods;
using ToDoApp.Core.Interfaces;



namespace ToDoApp.Api.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public partial class UserTaskController( IUnitOfWork unitOfWork) : ControllerBase
{

    private readonly IUnitOfWork _unitOfWork = unitOfWork;


    // GET: api/<ValuesController>
    [HttpGet("GetTasks", Name = "GetUserTasks")]
    public IActionResult GetUserTasks()
    {
        var result = _unitOfWork.UserTasks.GetUserTasks(Request).Serialize();
        return StatusCode(200,result);
    }

    //GET api/<ValuesController>/5
    [HttpGet("{task_id}", Name = "GetTaskById")]
    public IActionResult Get(int task_id)
    {
      return Ok(_unitOfWork.UserTasks.GetTaskById(Request,task_id));
    }



    //POST api/<ValuesController>
    [HttpPost("CreateTaskDTO", Name = "CreateTaskForUser")]
    public IActionResult Post([FromBody] CreateTaskDTO req)
    {
         _unitOfWork.UserTasks.CreateTask(Request , req);
        _unitOfWork.Complete();
        return Created();
    }


    //PUT api/<ValuesController>/5
    [HttpPut("UpdateTask", Name = "TaskDTO")]
    public IActionResult Put([FromBody] UpdateTaskDTO request)
    {
        var result = _unitOfWork.UserTasks.UpdateTask(Request, request);
        _unitOfWork.Complete();
        return Ok(result);
    }


    //PUT api/<ValuesController>/5
    [HttpPut("UpdateTaskStatus", Name = "UpdateTaskStatus")]
    public IActionResult UpdateTaskStatus([FromBody] PutTaskStatusDTO request)
    {
        var result = _unitOfWork.UserTasks.UpdateTaskState(Request, request);
        _unitOfWork.Complete();
        return Ok(result);
    }

    // DELETE api/<ValuesController>/5
    [HttpDelete("DeleteTask/{Id}")]
    public IActionResult Delete(int taskId)
    {
        _unitOfWork.UserTasks.DeleteTask(Request, taskId);
        _unitOfWork.Complete();
        return StatusCode(202,ExceptionMessages.SUCCESS);
    }



}

