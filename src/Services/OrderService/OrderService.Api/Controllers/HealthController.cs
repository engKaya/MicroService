﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace OrderService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
       [Route("api/health")]
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