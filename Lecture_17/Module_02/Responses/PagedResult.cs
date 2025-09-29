namespace Module_02.Responses;

public class PagedResult<T>
{
    public IEnumerable<T>? Items { get; set; }
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
}