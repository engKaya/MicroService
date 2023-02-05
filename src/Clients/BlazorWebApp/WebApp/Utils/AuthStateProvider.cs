using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace WebApp.Utils
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
