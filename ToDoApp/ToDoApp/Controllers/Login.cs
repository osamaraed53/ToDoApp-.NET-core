using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoApp.Data;
using ToDoApp.Helpers;
using ToDoApp.Method;
using static ToDoApp.Controllers.RegistrationController;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(IConfiguration config, ToDoAppDataContext context) : ControllerBase
    {
        protected readonly IConfiguration _config = config;
        protected readonly ToDoAppDataContext _context = context;

        public struct Login
        {
            [EmailAddress(ErrorMessage = "Invalid email address format.")]
            public string Email { get; set; }
            [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
              ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
            public string Password { get; set; }
        }


        [HttpPost(Name = "Login")]
        public async Task<IActionResult> Post([FromBody] Login reqData)
        {
            User UserInDb;
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {

                if (!CheckEmail.CheckIfUserExistByEmail(_context, reqData.Email))
                    return StatusCode(404, "User doesn't exist!!");
                else
                {

                    UserInDb = CheckEmail.DataForUserByEmail(_context, reqData.Email)!;
                    bool checkPassword = BCrypt.Net.BCrypt.EnhancedVerify(reqData.Password, UserInDb.Hash_password);
                    if (!checkPassword) { return StatusCode(400, "password wrong"); }
                }


                var token = JwtSecurityTokenHandlerWrapper.GenerateJwtToken(_config, UserInDb.User_id.ToString());



                return Ok((token = token ?? " "));
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }




    }
}
