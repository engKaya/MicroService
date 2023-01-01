using System.Collections.Generic;

namespace CatalogService.Api.Core.App.ViewModels
{
    public class PaginatedViewModel<TEntity> where TEntity : class
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long Count { get; set; }
        public IEnumerable<TEntity> Data { get; set; }

        public PaginatedViewModel(int pageIndex, int pageSize, long count, IEnumerable<TEntity> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }
    }
}
