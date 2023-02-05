using System.Collections.Generic;
using System.Net;

namespace WebApp.Domain.Models.User
{
    public class UserLoginResponse
    {
        public UserLoginResponse()
        {
            Rights = new List<string>();
        }
        public string UserName { get; set; }
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
        public List<string> Rights { get; set; }
        public int RoleId { get; set; }
        public string Message { get; set; } 
        public HttpStatusCode Status { get; set; }
    }
}
