
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BasketService.Api.Core.App.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public IdentityService(IHttpContextAccessor _httpContextAccessor)
        {
            this.httpContextAccessor = _httpContextAccessor;
        }
        public string GetUserName()
        {
            return httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}
