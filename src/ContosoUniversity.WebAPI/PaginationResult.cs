using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.WebAPI
{
    public class PaginationResult<T>(int pageIndex, int pageSize, int totalCount, List<T> items) where T : class
    {
        public int PageIndex { get; set; } = pageIndex;
        public int PageSize { get; set; } = pageSize;
        public int TotalCount { get; set; } = totalCount;
        public List<T> Items { get; set; } = items;
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginationResult<T>> Create(int pageIndex, int pageSize, IQueryable<T> source)
        {
            var totalCount = await source.CountAsync();
            var items = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
            return new PaginationResult<T>(pageIndex, pageSize, totalCount, items);
        }
    }
}
