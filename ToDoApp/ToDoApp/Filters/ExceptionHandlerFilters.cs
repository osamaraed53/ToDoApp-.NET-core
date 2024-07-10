using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using ToDoApp.Core.CustomExceptions;
using ToDoApp.Api.Filters;

public class ExceptionHandlerFilters : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
        context.ExceptionHandled = true;
    }

    private static void HandleException(ExceptionContext context)
    {
        var exception = context.Exception;
        HttpStatusCode code = exception switch
        {
            NotFoundException => HttpStatusCode.NotFound,
            ForbiddenException => HttpStatusCode.Forbidden,
            BadRequestException => HttpStatusCode.BadRequest,
            NotAllowedException => HttpStatusCode.NotAcceptable,
            _ => HttpStatusCode.InternalServerError,
        };

        SetExceptionResult(context, exception, code);
    }

    private static void SetExceptionResult(ExceptionContext context, Exception exception, HttpStatusCode code)
    {
        var response = new ApiResponse((int)code, exception.Message);
        context.Result = new JsonResult(response)
        {
            StatusCode = (int)code
        };
    }
}
