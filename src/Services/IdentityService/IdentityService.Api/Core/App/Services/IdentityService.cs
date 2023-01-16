using IdentityService.Api.Core.App.Models;
using IdentityService.Api.Core.Domain;
using IdentityService.Api.Extensions.Hashing;
using IdentityService.Api.Infastructure.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Api.Core.App.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IdentityContext context;
        private readonly IConfiguration configuration;
        private readonly int ExpiresValue;
        private readonly string SecretKey;
        public IdentityService(IdentityContext _context, IConfiguration _configuration)
        {
            this.context = _context;
            this.configuration = _configuration;
            this.ExpiresValue = int.Parse(configuration["CustomSettings:ExpiresIn"]);
            this.SecretKey = configuration["CustomSettings:Key"];
        }
        public Task<LoginResponseModel> Login(LoginRequestModels login)
        {
            var user = FindUserByStUsername(login.UserName);
            if (user == null)
            {
                return Task.FromResult(new LoginResponseModel
                {
                    Status = HttpStatusCode.NotFound,
                    Message = "Invalid Password Or Username"
                });
            }


            var checkUser = CheckUser(user, login.Password);
            if (!checkUser)
                return Task.FromResult(new LoginResponseModel()
                {
                    Status = HttpStatusCode.Unauthorized,
                    Message = "Invalid Password Or Username"
                });

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var creeds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expire = System.DateTime.Now.AddDays(10);
            var token = new JwtSecurityToken(claims: claims, expires: expire, signingCredentials: creeds, notBefore: DateTime.Now);
            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            var response = new LoginResponseModel
            {
                UserName = user.UserName,
                Token = encodedToken,
                ExpiresIn = ExpiresValue, 
                RoleId = user.Id,
                Status = HttpStatusCode.OK,
                Message = "Login is successful"
            };
            return Task.FromResult(response);
        }

        public bool CheckUser(User user, string password)
        {
            return PasswordHasher.VerifyHashedPassword(user.Password, password);
        }
        public User FindUserByStUsername(string StUsername) => context.Users.FirstOrDefault(i => i.UserName == StUsername);
    }
}
