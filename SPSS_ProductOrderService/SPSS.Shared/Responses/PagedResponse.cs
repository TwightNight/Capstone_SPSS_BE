namespace SPSS.Shared.Responses;

public class PagedResponse<TItem>
{
    public IReadOnlyCollection<TItem> Items { get; init; }
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }

    public int TotalPages => (PageSize > 0)
        ? (int)Math.Ceiling(TotalCount / (double)PageSize)
        : 0;

    public PagedResponse(IEnumerable<TItem> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items.ToList().AsReadOnly();
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public static PagedResponse<TItem> Empty(int pageNumber, int pageSize)
    {
        return new PagedResponse<TItem>(new List<TItem>(), 0, pageNumber, pageSize);
    }
}