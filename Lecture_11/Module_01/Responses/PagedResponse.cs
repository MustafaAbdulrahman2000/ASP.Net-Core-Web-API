using System.Runtime.CompilerServices;
using System.Threading.Tasks.Dataflow;

namespace Module_01.Responses;

public class PagedResponse<T>
{
    public IEnumerable<T> Items { get; set; }
    
    public PagedResponse(int totalCount, int pageSize) 
    {
            this.TotalCount = totalCount;
            this.PageSize = pageSize;
   
    }
    
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int) Math.Ceiling(TotalCount / (double) PageSize);

    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;

    private PagedResponse() {}

    public static PagedResponse<T> Create(IEnumerable<T> items, int totalCount, int page, int pageSize)
    {
        return new PagedResponse<T>
        {
            Items = items,
            TotalCount = totalCount,
            CurrentPage = page,
            PageSize = pageSize
        };
    }
}