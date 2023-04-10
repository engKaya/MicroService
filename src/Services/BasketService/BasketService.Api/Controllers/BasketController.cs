using BasketService.Api.Core.App.Repository;
using BasketService.Api.Core.App.Services;
using BasketService.Api.Core.Domain.Models;
using BasketService.Api.IntegrationEvents.Events;
using EventBus.Base.Abstraction;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BasketService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;
        private readonly IIdentityService identityService;
        private readonly IEventBus eventBus;
        private readonly ILogger<BasketController> logger;

        public BasketController(IBasketRepository _basketRepository, IIdentityService _identityService, IEventBus _eventBus, ILogger<BasketController> _logger)
        {
            this.basketRepository = _basketRepository;
            this.identityService = _identityService;
            this.eventBus = _eventBus;
            this.logger = _logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok($"{GetAssemblyName()} is healthy");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerBasket), 200)]
        public async Task<IActionResult> GetBasketById()
        {
            var id = identityService.GetUserName();
            var basket = await basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(CustomerBasket), 200)]
        public async Task<IActionResult> UpdateBasket([FromBody] CustomerBasket basket)
        {
            var updatedBasket = await basketRepository.UpdateBasketAsync(basket);
            return Ok(updatedBasket);
        }

        [Route("AddItem")]
        [HttpPost]
        [ProducesResponseType(typeof(CustomerBasket), 200)]
        public async Task<IActionResult> AddItemToBasket([FromBody] BasketItem item)
        {
            var basket = await basketRepository.GetBasketAsync(identityService.GetUserName());
            if (basket == null)
            {
                basket = new CustomerBasket(identityService.GetUserName());
            }

            var existingItem = basket.Items.Find(x => x.ProductId == item.ProductId);
            if (existingItem != null)
                existingItem.Quantity += item.Quantity;
            else
                basket.Items.Add(item);
            
            var updatedBasket = await basketRepository.UpdateBasketAsync(basket);
            return Ok(updatedBasket);
        }

        [Route("Checkout")]
        [HttpPost]
        [ProducesResponseType(typeof(CustomerBasket), 200)]
        [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CheckoutAsync([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await basketRepository.GetBasketAsync(basketCheckout.BuyerId);
            if (basket == null)
            {
                return BadRequest();
            }

            var basketRemoved = await basketRepository.DeleteBasketAsync(basketCheckout.BuyerId);
            if (!basketRemoved)
            {
                return BadRequest();
            }

            // send checkout event to rabbitmq
            var eventMessage = new OrderCreatedIntegrationEvent(
                    basketCheckout.BuyerId,
                    identityService.GetUserName(),
                    basketCheckout.City,
                    basketCheckout.Street,
                    basketCheckout.State,
                    basketCheckout.Country,
                    basketCheckout.ZipCode,
                    basketCheckout.CardNumber,
                    basketCheckout.CardHolderName,
                    basketCheckout.CardExpiration,
                    basketCheckout.CardSecurityNumber,
                    basketCheckout.CardTypeId,
                    basketCheckout.BuyerId,
                    basket
                );
            try
            {
                eventBus.Publish(eventMessage);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ERROR Publishing integration event: {EventId} from {AppName}", eventMessage.Id, GetAssemblyName());
                throw;
            }

            return Accepted();
        }
        [HttpDelete]
        [Route("DeleteBasket")]
        [ProducesResponseType(typeof(CustomerBasket), 200)]
        [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteBasketAsync()
        {
            var id = identityService.GetUserName();
            var deletedBasket = await basketRepository.DeleteBasketAsync(id);
            if (!deletedBasket)
                return BadRequest($"An error occured while deleting {id} basket!");

            return Ok($"{id} basket has been removed!");
        }

        [HttpGet]
        [Route("GetBasketCount")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> GetBasketCount()
        {
            var id = identityService.GetUserName();
            var basket = await basketRepository.GetBasketAsync(id);
            return Ok(basket?.Items.Count ?? 0);
        }


        private string GetAssemblyName()
        {
            return GetType().Assembly.GetName().Name;
        }
    }
}
