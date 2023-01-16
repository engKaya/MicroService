using BasketService.Api.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BasketService.Api.Core.App.Repository
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string customerId);
        IEnumerable<string> GetUsers();
        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
        Task<bool> DeleteBasketAsync(string Id);
    }
}
