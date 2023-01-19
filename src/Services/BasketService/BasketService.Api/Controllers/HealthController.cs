using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasketService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
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
