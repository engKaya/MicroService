using IdentityService.Api.Extensions.Hashing;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Api.Core.Domain
{
    public class User
    {
        public User()
        {

        }
        public User(string pass)
        {
            this.Password = PasswordHasher.HashPassword(pass);
        }
        public int Id { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; } 
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}"; 
    }
}
