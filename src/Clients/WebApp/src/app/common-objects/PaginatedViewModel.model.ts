export class PaginatedViewModel<TEntity> {
    PageIndex: number = 0;
    PageSize: number = 10;
    Count: number = 0;
    Data: TEntity[] = [];
}