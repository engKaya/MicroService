using IdentityService.Api.Core.App.Models;
using IdentityService.Api.Core.App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace IdentityService.Api.Controller
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService identityService;

        public AuthController(IIdentityService _identityService)
        {
            identityService = _identityService;
        }
        [HttpPost] 
        [ProducesResponseType(typeof(LoginRequestModels), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(LoginRequestModels), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(LoginRequestModels), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(LoginRequestModels), (int)HttpStatusCode.Unauthorized)]      
        
        public async Task<IActionResult> Login([FromBody] LoginRequestModels login)
        {
            var response = await identityService.Login(login);
            if (response.Status == HttpStatusCode.OK)
                return Ok(response);
            else if (response.Status == HttpStatusCode.NotFound)
                return NotFound(response);
            else if (response.Status == HttpStatusCode.Unauthorized)
                return Unauthorized(response);
            else
                return BadRequest(response);
        }
    }
}
