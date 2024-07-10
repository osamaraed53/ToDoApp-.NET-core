namespace ToDoApp.Core.Constant;

public static class RegularExpressionsConstant
{
    public const string Password = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
    public const string PasswordMessage = "Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character.";
    public const string EmailMessage = "Invalid email address format.";
}
