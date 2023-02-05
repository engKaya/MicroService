using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Features.Queries.GetOrderDetailById;
using System;
using System.Threading.Tasks;

namespace OrderService.Api.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ActionName("GetOrderDetailsById")]
        public async Task<IActionResult> GetOrderDetailsById()
        {
            var res = await _mediator.Send(new GetOrderDetailsQuery(new Guid()));
            return Ok(res);
        }
    }
}
