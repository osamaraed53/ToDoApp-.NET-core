namespace ToDoApp.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IUserTaskRepository UserTasks { get; }
    int Complete();
}
