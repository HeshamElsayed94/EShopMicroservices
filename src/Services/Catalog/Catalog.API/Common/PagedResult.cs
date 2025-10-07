namespace Catalog.API.Common;

public sealed record PagedResult<T>(
    long TotalCount,
    long TotalPages,
    long CurrentPage,
    long PageSize,
    bool HasNextPage,
    bool HasPreviousPage,
    IEnumerable<T> Items
) where T : notnull
{

    public PagedResult(IPagedList<T> pagedList) : this(
        pagedList.TotalItemCount,
        pagedList.PageCount,
        pagedList.PageNumber,
        pagedList.PageSize,
        pagedList.HasNextPage,
        pagedList.HasPreviousPage,
        pagedList
    )
    { }
}