﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
