using Microsoft.AspNetCore.Mvc.Filters;

namespace ToDoApp.Api.Filters;

public class PremissionBasedAuthorizationFilter : IAsyncAuthorizationFilter
{
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        throw new NotImplementedException();
    }
}
