using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;

namespace BasketService.Api.Extensions
{
    public static class RedisRegistration
    {
        public static ConnectionMultiplexer AddRedis(this IServiceProvider services, IConfiguration configuration)
        {
            var redisConf = ConfigurationOptions.Parse(configuration["CustomSettings:RedisConnection"], true);
            redisConf.ResolveDns = true;
            return ConnectionMultiplexer.Connect(redisConf);
        }
    }
}
