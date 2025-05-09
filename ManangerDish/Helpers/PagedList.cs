// Trong thư mục Models hoặc một thư mục Helper riêng
using Microsoft.EntityFrameworkCore;

public class PagedList<T>
{
    public int PageNumber { get; private set; }
    public int PageSize { get; private set; }
    public int TotalItems { get; private set; }
    public int TotalPages { get; private set; }
    public List<T> Items { get; private set; }

    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    private PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItems = count;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        Items = items;
    }

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize < 1 ? 10 : pageSize; 

        var count = await source.CountAsync(); 

        var items = await source.Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}