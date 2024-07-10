namespace ToDoApp.Core.CustomExceptions;

public class DbException : Exception
{
    public DbException() { }
    public DbException(string msg)
        : base(msg) { }

}
