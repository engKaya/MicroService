using CatalogService.Api.Core.App.ViewModels;
using CatalogService.Api.Core.Domain;
using CatalogService.Api.Infastructure;
using CatalogService.Api.Infastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogContext _catalogContext;
        private readonly CatalogSettings _settings;

        public CatalogController(CatalogContext catalogContext, IOptionsSnapshot<CatalogSettings> settings)
        {
            _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
            _settings = settings.Value;

            ((CatalogContext)_catalogContext).ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(PaginatedViewModel<CatalogItem>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<CatalogItem>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ItemsAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0, string ids = null)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = await GetItemsByIdsAsync(ids);
                if (!items.Any())
                {
                    return BadRequest("ids value invalid, Must be comma seperated list of numbers");
                }

                return Ok(items);
            }

            var totalItems = await _catalogContext.CatalogItems.LongCountAsync();
            var itemsOnPage = await _catalogContext.CatalogItems
                    .OrderBy(x => x.Name)
                    .Skip(pageSize * pageIndex)
                    .Take(pageSize)
                    .ToListAsync();

            var model = new PaginatedViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }
        private async Task<List<CatalogItem>> GetItemsByIdsAsync(string ids)
        {
            var numIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out var number), Value: number));
            if (!numIds.All(x => x.Ok))
                return new List<CatalogItem>();

            var idsToSelect = numIds.Select(x => x.Value);
            var items = await _catalogContext.CatalogItems.Where(x => idsToSelect.Contains(x.Id)).ToListAsync();
            items = ChangeUriPlaceholder(items);

            return items;
        }

        [HttpGet]
        [Route("items/{id:int}")]
        [ProducesResponseType(typeof(CatalogItem), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<CatalogItem>> ItemById(int id)
        {
            if (id <= 0)
                return BadRequest($"Wrong {nameof(id)} value!");

            var item = await _catalogContext.CatalogItems.SingleOrDefaultAsync(x => x.Id == id);
            if (item == null)
                return NotFound($"Item not found with given {nameof(id)}");

            var baseUri = _settings.PicBaseUrl;

            item.PictureUri = baseUri + item.PictureFileName;
            return item;
        }

        [HttpGet]
        [Route("items/withname/{name:minlength(1)}")]
        [ProducesResponseType(typeof(CatalogItem), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<PaginatedViewModel<CatalogItem>>> ItemsWithNameAsync(string name, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            if (string.IsNullOrEmpty(name))
                BadRequest($"Wrong {nameof(name)} value!");

            var totalItems = await _catalogContext.CatalogItems
                .Where(x => x.Name.StartsWith(name))
                .LongCountAsync();

            var itemsOnPage = await _catalogContext.CatalogItems
                .Where(x => x.Name.StartsWith(name))
                .OrderBy(x => x.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            var model = new PaginatedViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);
            return Ok(model);
        }

        [HttpGet]
        [Route("items/type/{catalogTypeId}/brand/{catalogBrandId:int?}")]
        [ProducesResponseType(typeof(PaginatedViewModel<CatalogItem>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedViewModel<CatalogItem>>> ItemsByTypeId(int catalogType, int? catalogBrandId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var root = (IQueryable<CatalogItem>)_catalogContext.CatalogItems;
            root = root.Where(x => x.CatalogTypeId == catalogType);
            
            if (catalogBrandId.HasValue)
                root = root.Where(x => x.CatalogBrandId == catalogBrandId);

            var totalItems = await root
                .LongCountAsync();

            var itemsOnpage = await root
                    .Skip(pageSize * pageIndex)
                    .Take(pageSize)
                    .ToListAsync();

            itemsOnpage = ChangeUriPlaceholder(itemsOnpage);

            return new PaginatedViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnpage);
        }
        [HttpGet]
        [Route("items/type/all/brand/{catalogBrandId:int?}")]
        [ProducesResponseType(typeof(PaginatedViewModel<CatalogItem>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedViewModel<CatalogItem>>> ItemsByBrandId(int? catalogBrandId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var root = (IQueryable<CatalogItem>)_catalogContext.CatalogItems; 

            if (catalogBrandId.HasValue)
                root = root.Where(x => x.CatalogBrandId == catalogBrandId);

            var totalItems = await root
                .LongCountAsync();

            var itemsOnpage = await root
                    .Skip(pageSize * pageIndex)
                    .Take(pageSize)
                    .ToListAsync();

            itemsOnpage = ChangeUriPlaceholder(itemsOnpage);

            return new PaginatedViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnpage);
        }

        [HttpGet]
        [Route("catalogtypes")] 
        [ProducesResponseType(typeof(List<CatalogType>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<CatalogType>>> CatalogTypesAsync()
        {
            return await _catalogContext.CatalogTypes.ToListAsync();
        }

        [HttpGet]
        [Route("catalogBrands")]
        [ProducesResponseType(typeof(List<CatalogBrand>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<CatalogBrand>>> CatalogBrandsAsync()
        {
            return await _catalogContext.CatalogBrands.ToListAsync();
        }

        [Route("items")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> UpdateItemAsync([FromBody] CatalogItem item)
        {
            var catalogItem = await _catalogContext.CatalogItems.SingleOrDefaultAsync(x => x.Id == item.Id);
            if (catalogItem == null)
                return NotFound(new { Message = $"Item with id {item.Id} not found." });

            catalogItem = item;
            _catalogContext.CatalogItems.Update(catalogItem);
            await _catalogContext.SaveChangesAsync();

            return CreatedAtAction(nameof(ItemById), new { id = item.Id }, null);
        } 

        [Route("items")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateProductAsync([FromBody] CatalogItem item)
        {
            var itemToAdd = new CatalogItem
            {
                CatalogBrandId = item.CatalogBrandId,
                CatalogTypeId = item.CatalogTypeId,
                Description = item.Description,
                Name = item.Name,
                Price = item.Price,
                PictureFileName = item.PictureFileName,
                AvailableStock = item.AvailableStock
            };

            _catalogContext.CatalogItems.Add(itemToAdd);
            await _catalogContext.SaveChangesAsync();

            return CreatedAtAction(nameof(ItemById), new { id = itemToAdd.Id }, null);
        }

        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var item = await _catalogContext.CatalogItems.SingleOrDefaultAsync(x => x.Id == id);
            if (item == null)
                return NotFound();

            _catalogContext.CatalogItems.Remove(item);
            await _catalogContext.SaveChangesAsync();

            return NoContent();
        }


        private List<CatalogItem> ChangeUriPlaceholder(List<CatalogItem> items)
        {
            var baseUri = _settings.PicBaseUrl;
            items.ForEach(x => x.PictureUri = baseUri + x.PictureFileName);
            return items;
        }
    }
}
