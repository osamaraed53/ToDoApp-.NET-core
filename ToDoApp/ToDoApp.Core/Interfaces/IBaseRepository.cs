using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace ToDoApp.Core.Interfaces;

public interface IBaseRepository<T> where T : class
{

    T GetById(int id);

    T Find(Expression<Func<T, bool>> exception);

    IEnumerable<T> FindAll(Expression<Func<T, bool>> exception);
    T Add(T entity);

    Task<T> AddAsync(T entity);

    T Update(T entity);

    bool Delete(T entity);
    IDictionary<string, string> ValidateJwtToken(string token);

    int GetUserId(HttpRequest requestHeader);


}
