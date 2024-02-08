using ToDoApp.Data;

namespace ToDoApp.Method
{
    public static class CheckEmail
    {
        public static bool CheckIfUserExistByEmail(ToDoAppDataContext context, string email)
        {
            var value = context.Users.Where(user => user.Email == email).FirstOrDefault();
            Console.WriteLine(value?.Username);
            return value != null ? true : false;
        }


        public static User? DataForUserByEmail(ToDoAppDataContext context, string email)
        {
            var value = context.Users.Where(user => user.Email == email).FirstOrDefault();
            User? result = value;
            return result;
        }

    }
}
