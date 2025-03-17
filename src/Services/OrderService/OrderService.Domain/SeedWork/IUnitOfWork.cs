using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Domain.SeedWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> SaveChangesAsync(CancellationToken cancellation = default(CancellationToken));
        Task<bool> SaveEntitiesAsync(CancellationToken cancellation = default(CancellationToken));
    } 
}
