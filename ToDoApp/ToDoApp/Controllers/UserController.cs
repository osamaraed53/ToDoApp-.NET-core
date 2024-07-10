using Microsoft.AspNetCore.Mvc;
using ToDoApp.Core.Constant;
using ToDoApp.Core.DTOs;
using ToDoApp.Core.ExtensionMethods;
using ToDoApp.Core.Interfaces;



namespace ToDoApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IConfiguration config, IUnitOfWork unitOfWork) : ControllerBase
{
    protected readonly IConfiguration _config = config;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    [HttpPost("Login", Name = "Login")]
    public IActionResult Post([FromBody] LoginDTO req)
    {
        if (!ModelState.IsValid) return StatusCode(400, ModelState["errors"]);
        string token = _unitOfWork.Users.Login(req.Email, req.Password);
        return Ok(token);
    }






    [HttpPost("SignUp", Name = "SignUp")]
    public IActionResult Post([FromBody] SignUpDTO res)
    {
        if (!ModelState.IsValid) return StatusCode(400, ModelState["errors"]);

        _unitOfWork.Users.SignUp(res.Username, res.Email, res.Password);
        //TODO : cheack what is return
        _unitOfWork.Complete();

        return StatusCode(201,new { message = ExceptionMessages.SUCCESS}.Serialize());

    }

}
