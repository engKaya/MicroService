using BasketService.Api.Core.App.Repository;
using BasketService.Api.Core.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketService.Api.Infastructure.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly ILogger<BasketRepository> logger;
        private readonly ConnectionMultiplexer redis;
        private readonly IDatabase database;

        public BasketRepository(ConnectionMultiplexer _redis, ILogger<BasketRepository> _logger)
        {
            this.redis = _redis;
            this.logger = _logger;
                database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string Id)
        {
            return await database.KeyDeleteAsync(Id);
        }

        public async Task<CustomerBasket> GetBasketAsync(string customerId)
        {
            var data = await database.StringGetAsync(customerId);
            if (data.IsNullOrEmpty)
                return null;

            return JsonConvert.DeserializeObject<CustomerBasket>(data);
        }

        public IEnumerable<string> GetUsers()
        { 
            var server = GetServer();
            var data = server.Keys();
            return data?.Select(x => x.ToString());
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var created = await database.StringSetAsync(basket.BuyerId, JsonConvert.SerializeObject(basket));
            if (!created)
            {
                logger.LogInformation("Problem occured while setting basket");
                return null;
            }

            logger.LogInformation("Item Successfully Persisted!");
            return await GetBasketAsync(basket.BuyerId);
        }

        private IServer GetServer()
        {
            var endpoint = redis.GetEndPoints();
            return redis.GetServer(endpoint.FirstOrDefault());
        }
    }
}
