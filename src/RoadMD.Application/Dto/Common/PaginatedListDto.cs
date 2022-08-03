using Microsoft.EntityFrameworkCore;

namespace RoadMD.Application.Dto.Common
{
    public class PaginatedListDto<T> where T : class
    {
        public List<T> Items { get; }
        public int PageIndex { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }

        public PaginatedListDto(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedListDto<T>> CreateAsync(IQueryable<T> source, int? pageNumber, int? pageSize,
            CancellationToken cancellationToken = default)
        {
            var count = await source
                .AsNoTracking()
                .CountAsync(cancellationToken: cancellationToken);

            pageNumber = pageNumber.GetValueOrDefault(1);
            pageSize = pageSize.GetValueOrDefault(50);

            var items = await source.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value)
                .ToListAsync(cancellationToken: cancellationToken);

            return new PaginatedListDto<T>(items, count, pageNumber.Value, pageSize.Value);
        }
    }
}