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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetOrderDetailsById(Guid id)
        {
            var res = await _mediator.Send(new GetOrderDetailsQuery(id));
            return Ok(res);
        }
    }
}
