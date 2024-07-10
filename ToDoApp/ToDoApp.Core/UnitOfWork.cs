using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using ToDoApp.Core.Interfaces;
using ToDoApp.Core.Repositories;
using ToDoApp.EF.Data;

namespace ToDoApp.Core;

public class UnitOfWork : IUnitOfWork
{

    private readonly AppDbContext _context;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
    private readonly IConfiguration _configuration;
    public IUserRepository Users { get; private set; }
    public IUserTaskRepository UserTasks { get; private set; }


    public UnitOfWork(IConfiguration Configuration, AppDbContext context, JwtSecurityTokenHandler JwtSecurityTokenHandler)
    {
        _context = context;
        _jwtSecurityTokenHandler = JwtSecurityTokenHandler;
        _configuration = Configuration;
        Users = new UserRepository(_configuration, _context, _jwtSecurityTokenHandler);
        UserTasks = new UserTaskRepository(_configuration, _context, _jwtSecurityTokenHandler);
    }


    public int Complete() => _context.SaveChanges();

    public void Dispose() => _context.Dispose();

}
