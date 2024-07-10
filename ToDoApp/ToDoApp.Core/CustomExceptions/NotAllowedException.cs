using System.Runtime.Serialization;

namespace ToDoApp.Core.CustomExceptions;

[Serializable]
public class NotAllowedException : Exception
{
    public NotAllowedException()
    {
    }

    public NotAllowedException(string? message) : base(message)
    {
    }

    public NotAllowedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }


}