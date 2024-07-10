namespace ToDoApp.EF.test;

public class Result
{
    public object? Value { get; set; }
    public int? StatusCode { get; set; }
    public Result(int statusCode)
    {
        StatusCode = statusCode;
    }
    public Result(int statusCode, object value)
    {
        StatusCode = statusCode;
        Value = value;
    }
}
