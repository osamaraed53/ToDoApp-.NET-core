using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoApp.Core.Constant;
using ToDoApp.Core.CustomExceptions;
using ToDoApp.Core.Interfaces;
using ToDoApp.EF.Data;
using ToDoApp.EF.Models;


namespace ToDoApp.Core.Repositories;

public class UserRepository
    (IConfiguration configuration, AppDbContext context, JwtSecurityTokenHandler jwtSecurityTokenHandler)
    : BaseRepository<User>(configuration, context, jwtSecurityTokenHandler), IUserRepository
{
    private readonly IConfiguration _configuration = configuration;
    private readonly AppDbContext _context = context;



    public string Login(string email, string password)
    {
        if (CheckIfUserExistByEmail(email))
            throw new NotFoundException(ExceptionMessages.NOT_EXIST_ERROR);

        if (!CanLogin(password, email, out string token))
            throw new BadRequestException(ExceptionMessages.USER_DATA_INCORRECT);
        return token;

    }
   
    public void SignUp(string username, string email, string password)
    {
        if (!CheckIfUserExistByEmail(email)) throw new BadRequestException(ExceptionMessages.ALREADY_EXIST);
        string HashPassword = HashingPassword(password);
        var new_user = new User() { Username = username, HashPassword = HashPassword, Email = email };
        Add(new_user);
    }

    public bool CheckIfUserExistByEmail(string email)
    {
        var value = _context.Users.Where(user => user.Email == email).FirstOrDefault();
        return value != null ? false : true;
    }

    public bool CanLogin(string password, string email, out string token)
    {
        token = string.Empty;
        var userData = _context.Users.SingleOrDefault(user => user.Email == email);

        if (userData == null)
            return false;

        bool isPasswordValid = ValidatePassword(password, userData.HashPassword);

        if (isPasswordValid)
        {
            token = generateJwtToken(userData.Id.ToString());
            return true;
        }
        return false;
    }

    public string generateJwtToken(string userId)
    {
        try
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var Sectoken = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"],

             claims: new List<Claim>() { new("Id", userId), },
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
            return token;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public string HashingPassword(string password)
    {
        // TODO : Should ADD 13 to Constatnt class
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
    }
    public bool ValidatePassword(string passwordFromReq, string hashPasswordFromDb)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(passwordFromReq, hashPasswordFromDb);
    }



}
