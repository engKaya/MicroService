using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.AggregateModels.BuyerAggregate;
using OrderService.Infastructure.Context;

namespace OrderService.Infastructure.Repositories
{
    public class BuyerRepository : GenericRepository<Buyer>, IBuyerRepository
    {
        public BuyerRepository(OrderDbContext dbContext) : base(dbContext)
        {
        }
    }
}
