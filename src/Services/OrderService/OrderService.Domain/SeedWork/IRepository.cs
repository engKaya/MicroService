namespace OrderService.Domain.SeedWork
{
    public interface IRepository<T>  where T : BaseEntity
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
