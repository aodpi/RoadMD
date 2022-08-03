using RoadMD.Application.Dto.Common;

namespace RoadMD.Application.Common.Extensions
{
    public static class MappingExtensions
    {
        public static Task<PaginatedListDto<TDestination>> ToPaginatedListAsync<TDestination>(
            this IQueryable<TDestination> queryable, int? pageNumber, int? pageSize,
            CancellationToken cancellationToken = default) where TDestination : class
            => PaginatedListDto<TDestination>.CreateAsync(queryable, pageNumber, pageSize, cancellationToken);
    }
}