using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok($"{GetAssemblyName()} is healthy");
        }

        private string GetAssemblyName()
        {
            return GetType().Assembly.GetName().Name;
        }
    }
}
