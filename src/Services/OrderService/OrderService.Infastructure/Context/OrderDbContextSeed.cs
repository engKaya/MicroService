using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Domain.AggregateModels.BuyerAggregate;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Domain.SeedWork;
using Polly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infastructure.Context
{
    public class OrderDbContextSeed
    {
        public async Task SeedAsync(OrderDbContext orderDbContext, ILogger<OrderDbContext> logger)
        {

            string rootPath = Directory.GetCurrentDirectory();
            rootPath = rootPath.Replace("OrderService.Api", "OrderService.Infastructure");
            var policy = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(
                        retryCount: 5,
                        sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                        onRetry: (exception, timeSpan, retry, ctx) =>
                        {
                            logger.LogError(exception, $"Exception {exception.Message} occured on attempt {retry} of {ctx.PolicyKey}.");
                        }
                    );

            var setupDirPath = Path.Combine(rootPath, "Seeding", "Setup");
            await policy.ExecuteAsync(async () =>
            {
                var customizationData = true;

                orderDbContext.Database.Migrate();
                if (!orderDbContext.CardTypes.Any())
                {
                    orderDbContext.CardTypes.AddRange(customizationData ? GetCardTypesFromFile(setupDirPath) : GetPreDefinedCardData());
                }

                if (!orderDbContext.OrderStatuses.Any())
                {
                    orderDbContext.OrderStatuses.AddRange(customizationData ? GetOrderStatusesFromFile(setupDirPath) : GetPreDefinedStatusData());
                }
            });
        }

        private IEnumerable<CardType> GetCardTypesFromFile(string contentPath)
        {
            string fileName = Path.Combine(contentPath, "CardTypes.txt");
            var id = 1;
            var cardTypes = File.ReadAllLines(fileName)
                .Select(x => new CardType(id++, x)).Where(x => x != null).ToList();

            return cardTypes;
        }

        private IEnumerable<OrderStatus> GetOrderStatusesFromFile(string contentPath)
        {
            string fileName = Path.Combine(contentPath, "OrderStatus.txt");
            var id = 1;
            var statutes = File.ReadAllLines(fileName).Select(x => new OrderStatus(id++, x)).Where(x => x != null).ToList();
            return statutes;
        }


        private IEnumerable<CardType> GetPreDefinedCardData()
        {
            return Enumaration.GetAll<CardType>();
        }

        private IEnumerable<OrderStatus> GetPreDefinedStatusData()
        {
            return Enumaration.GetAll<OrderStatus>();
        }
    }
}
