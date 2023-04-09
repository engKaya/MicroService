using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Net;

namespace IdentityService.Api.Core.App.Models
{
    public class LoginResponseModel
    {
        public LoginResponseModel()
        {
            Rights = new List<string>();
        }
        public string UserName { get; set; }
        public string Token { get; set; }
        public int ExpiresIn { get; set; } 
        public List<string> Rights { get; set; }
        public int RoleId { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }

        public HttpStatusCode Status { get; set; }
    }
}
