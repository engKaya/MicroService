using CatalogService.Api.Core.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.Api.Infastructure.Context
{
    public class CatalogContextSeed
    {
        public async Task SeedAsync(CatalogContext context, IWebHostEnvironment environment, ILogger<CatalogContextSeed> logger)
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
            var picturePath = "Pics";

            await policy.ExecuteAsync(() =>  ProcessSeeding(context, setupDirPath, picturePath));
        }

        private async Task ProcessSeeding(CatalogContext context, string setupDirPath, string picturePath)
        {
            if (!context.CatalogBrands.Any())
            {
                await context.CatalogBrands.AddRangeAsync(GetCatalogBrandsFromFile(setupDirPath));
                await context.SaveChangesAsync();
            }

            if (!context.CatalogTypes.Any())
            {
                await context.CatalogTypes.AddRangeAsync(GetCatalogTypesFromFile(setupDirPath));
                await context.SaveChangesAsync();
            }

            if (!context.CatalogItems.Any())
            {
                await context.CatalogItems.AddRangeAsync(GetCatalogItemsFromFile(setupDirPath, context));
                await context.SaveChangesAsync();
                GetCatalogItemsPictures(setupDirPath, picturePath);
            }
        }

        private IEnumerable<CatalogBrand> GetCatalogBrandsFromFile(string contentPath)
        {
            string fileName = Path.Combine(contentPath, "CatalogBrands.txt");
            var fileContent = File.ReadAllLines(fileName);
            var list = fileContent.Select(i => new CatalogBrand()
            {
                Brand = i
            }).Where(i => i != null);

            return list;
        }

        private IEnumerable<CatalogType> GetCatalogTypesFromFile(string contentPath)
        {
            string fileName = Path.Combine(contentPath, "CatalogTypes.txt");
            var fileContent = File.ReadAllLines(fileName);
            var list = fileContent.Select(i => new CatalogType()
            {
                Type = i
            }).Where(i => i != null);

            return list;
        }
        private IEnumerable<CatalogItem> GetCatalogItemsFromFile(string contentPath, CatalogContext context)
        {
            string fileName = Path.Combine(contentPath, "CatalogItems.txt");

            var catalogTypeIdLookUp = context.CatalogTypes.ToDictionary(ct => ct.Type, ct => ct.Id);
            var catalogBrandsLookUp = context.CatalogBrands.ToDictionary(cb => cb.Brand, cb=> cb.Id);

            var fileContent = File.ReadAllLines(fileName)
                    .Skip(1)
                    .Select(i => i.Split(','))
                    .Select(i => new CatalogItem
                    {
                        CatalogTypeId = catalogTypeIdLookUp[i[0]],
                        CatalogBrandId = catalogBrandsLookUp[i[1]],
                        Description = i[2],
                        Name = i[3],
                        Price = decimal.Parse(i[4], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture),
                        PictureFileName = i[5],
                        AvailableStock = string.IsNullOrEmpty(i[6]) ? 0 : int.Parse(i[6]),
                        OnReorder = Convert.ToBoolean(i[7])
                    }); 
            return fileContent;
        }
        
        private void GetCatalogItemsPictures(string contentPath, string picturePath)
        {
            picturePath ??= "pics";
            DirectoryInfo directoryInfo = new DirectoryInfo(picturePath);
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }

            string zipPath = Path.Combine(contentPath, "CatalogItems.zip");
            ZipFile.ExtractToDirectory(zipPath, picturePath);
        }
    }
}
