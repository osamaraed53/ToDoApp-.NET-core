using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Text;
using ToDoApp.Core.Interfaces;
using ToDoApp.EF.Data;

namespace ToDoApp.Core.Repositories;

public class BaseRepository<T>(IConfiguration Configuration, AppDbContext context, JwtSecurityTokenHandler JwtSecurityTokenHandler) : IBaseRepository<T> where T : class
{
    private readonly IConfiguration _configuration = Configuration;
    private readonly AppDbContext _context = context;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = JwtSecurityTokenHandler;

    public T GetById(int id)
    {
        return _context.Set<T>().Find(id)!;
    }

    public T Find(Expression<Func<T, bool>> expression) => _context.Set<T>().FirstOrDefault(expression)!;

    public IEnumerable<T> FindAll(Expression<Func<T, bool>> expression)
    {
        IQueryable<T> quere = _context.Set<T>().Where(expression);

        return quere.ToList();
    }

    public T Add(T entity)
    {
        _context.Set<T>().Add(entity);
        return entity;
    }

    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        return entity;
    }

    public bool Delete(T entity)
    {
        try
        {
            _context.Remove(entity);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public IDictionary<string, string> ValidateJwtToken(string token)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]!));

        try
        {
            var tokenHandler = _jwtSecurityTokenHandler;

            // Add logging for debugging purposes

            var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidIssuer = _configuration["Jwt:Issuer"],
                IssuerSigningKey = securityKey,
            }, out SecurityToken validatedToken);

            string userId = claimsPrincipal.FindFirst("Id")?.Value!;

            Dictionary<string, string> payload =
            new Dictionary<string, string>(){
                                  {"user_id", userId},
                         };

            return payload;
        }
        catch (SecurityTokenExpiredException)
        {
            throw new ApplicationException("Token has expired.");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }



    public int GetUserId(HttpRequest requestHeader)
    {
        var token = requestHeader.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var payload = ValidateJwtToken(token);
        int user_id = short.Parse(payload["user_id"]);
        return user_id;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }

}
