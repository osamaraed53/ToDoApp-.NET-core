using Microsoft.AspNetCore.Mvc;
using ToDoApp.Data;
using ToDoApp.Method;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class RegistrationController(IConfiguration config, ToDoAppDataContext context) : ControllerBase
    {
        protected readonly IConfiguration _config = config;
        protected readonly ToDoAppDataContext _context = context;



        [HttpPost(Name = "SignUp")]
        public async Task<IActionResult> Post([FromBody] SignUp reqData)
        {

            if (!ModelState.IsValid) return StatusCode(400, ModelState["errors"]);

            try
            {

                string HashPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(reqData.Password, 13);
                var new_user = new User() { Username = reqData.Username, Hash_password = HashPassword, Email = reqData.Email };


                if (CheckEmail.CheckIfUserExistByEmail(_context, reqData.Email)) return StatusCode(400, "user Exist!!!!");

                try
                {
                    _context.Users.Add(new_user);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return StatusCode(500, "Internal Server Error!!!!!!!!!!!!");

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return StatusCode(201, "Signed Up Successfully");

        }










    }
}
