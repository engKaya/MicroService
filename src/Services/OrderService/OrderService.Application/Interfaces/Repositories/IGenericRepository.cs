using OrderService.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync();
        Task<List<T>> Get(Expression<Func<T, bool>> predicate = null, Func<IQueryable, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes);
        Task<List<T>> Get(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetById(Guid id);
        Task<T> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties);
        Task<T> AddAsync(T entity);
        T Update(T entity);
    }   
}
