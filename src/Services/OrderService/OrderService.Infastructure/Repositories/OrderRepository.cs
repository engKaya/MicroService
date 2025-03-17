using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Infastructure.Context;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OrderService.Infastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly OrderDbContext dbContext;
        public OrderRepository(OrderDbContext _dbContext) : base(_dbContext)
        {
            dbContext = _dbContext;
        }

        public override async Task<Order> GetByIdAsync(Guid id, params Expression<Func<Order, object>>[] includeProperties)
        {
            var entity = await base.GetByIdAsync(id, includeProperties);
            if (entity == null)
                entity = dbContext.Orders.Local.FirstOrDefault(x => x.Id == id);

            return entity;
        }
    } 
}
