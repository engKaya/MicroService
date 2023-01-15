using IdentityService.Api.Core.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Api.Infastructure.Context
{
    public class IdentityContextSeed
    {
        public async Task SeedAsync(IdentityContext identityContext, IWebHostEnvironment environment, ILogger<IdentityContextSeed> logger)
        {
            var policy = Policy.Handle<SqlException>()
                .WaitAndRetryAsync(
                    retryCount: 5,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogError(exception, $"Exception {exception.Message} occured on attempt {retry} of {ctx.PolicyKey}.");
                    }
                );
            var setupDirPath = Path.Combine(environment.ContentRootPath, "Infastructure", "Setup", "SeedFiles");

            await policy.ExecuteAsync(() => ProcessSeeding(identityContext, setupDirPath));
        }
        private async Task ProcessSeeding(IdentityContext context, string setupDirPath)
        {
            if (!context.Users.Any())
            {
                await context.Users.AddRangeAsync(GetUsersFromFile(setupDirPath));
                await context.SaveChangesAsync();
            }
        }

        private IEnumerable<User> GetUsersFromFile(string path)
        {
            string fileName = Path.Combine(path, "UsersSeed.txt");

            var fileContent = File.ReadAllLines(fileName) 
                    .Select(i => i.Split(','))
                    .Select(i =>
                    {
                        var user = new User(i[2])
                        {
                            Email = i[0],
                            UserName = i[1],
                            FirstName = i[3],
                            LastName = i[4]
                        }; 
                        return user;
                    });
            return fileContent;
        }
    }
}
