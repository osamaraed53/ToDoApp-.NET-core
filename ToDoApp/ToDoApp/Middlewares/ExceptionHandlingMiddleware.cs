using System.Net;
using System.Text.Json;
using ToDoApp.Api.Filters;
using ToDoApp.Core.CustomExceptions;

namespace ToDoApp.Api.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{


    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {   
        
        
        string result;
        HttpStatusCode code = exception switch
        {
            NotFoundException => HttpStatusCode.NotFound,
            ForbiddenException => HttpStatusCode.Forbidden,
            BadRequestException => HttpStatusCode.BadRequest,
            NotAllowedException => HttpStatusCode.NotAcceptable,
            _ => HttpStatusCode.InternalServerError,
        };
        var response = new ApiResponse((int)code, exception.Message);
        result = JsonSerializer.Serialize(response);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }


}
