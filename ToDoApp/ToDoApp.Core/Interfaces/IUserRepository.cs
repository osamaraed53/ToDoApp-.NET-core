using ToDoApp.EF.Models;



namespace ToDoApp.Core.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    bool CheckIfUserExistByEmail(string email);
    bool CanLogin(string password, string email, out string token);
    string HashingPassword(string password);
    bool ValidatePassword(string passwordFromReq, string hashPasswordFromDb);
    string Login(string email, string password);
    void SignUp(string username, string email, string password);


}
