namespace WebApp.Domain.Models.User
{
    public class UserLogingRequest
    {
        public string UserName { get; private set; }
        public string Password { get; private set; }

        public UserLogingRequest(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
