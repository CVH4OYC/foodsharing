namespace Foodsharing.API.DTOs;

public class PagedResponse<T>
{
    public List<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public PagedResponse(List<T> items, int totalCount, PaginationParams pagination)
    {
        Items = items;
        TotalCount = totalCount;
        CurrentPage = pagination.Page;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize);
    }
}
