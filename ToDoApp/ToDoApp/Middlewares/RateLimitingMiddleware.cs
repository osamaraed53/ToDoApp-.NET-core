namespace ToDoApp.Api.Middlewares;

public class RateLimitingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    private static int _counter = 0;
    private static DateTime _lastRequestDate = DateTime.Now;


    public async Task Invoke(HttpContext httpContext)
    {
        _counter++;
        if(DateTime.Now.Subtract(_lastRequestDate).Seconds > 10 ) {

            _counter = 1;
            _lastRequestDate = DateTime.Now;
            await _next(httpContext);     
        }else
        {
            if(_counter > 5)
            {
                _lastRequestDate = DateTime.Now;
                await httpContext.Response.WriteAsync("Rate Limit exceeded");
            }
            else
            {
                _lastRequestDate = DateTime.Now;
                await _next(httpContext);
            }
        }



    } 
}
