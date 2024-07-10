using System.Net;

namespace ToDoApp.Api.Filters;

public class ApiResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    //public object? Data { get; set; }

    public ApiResponse(Exception exception)
    {
        StatusCode = (int)HttpStatusCode.InternalServerError;
        Message = exception.Message;
    }

    public ApiResponse(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }

    //public ApiResponse(int statusCode, string message,object data)
    //{
    //    StatusCode = statusCode;
    //    Message = message;
    //    Data = data;
    //}
}
