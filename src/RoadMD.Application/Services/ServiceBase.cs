using Mapster;
using MapsterMapper;
using RoadMD.Application.Common.Extensions;
using RoadMD.Application.Dto.Common;
using RoadMD.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace RoadMD.Application.Services
{
    /// <summary>
    /// Base class used for services containing dbcontext
    /// and object mapper
    /// </summary>
    public abstract class ServiceBase
    {
        protected ServiceBase(ApplicationDbContext context, IMapper mapper, ISieveProcessor sieveProcessor)
        {
            Context = context;
            Mapper = mapper;
            SieveProcessor = sieveProcessor;
        }

        /// <summary>
        ///     Mapper from entity to Dto
        /// </summary>
        protected IMapper Mapper { get; init; }

        /// <summary>
        ///     Database context
        /// </summary>
        protected ApplicationDbContext Context { get; init; }

        /// <summary>
        ///     Sieve processor
        /// </summary>
        protected ISieveProcessor SieveProcessor { get; init; }


        protected async Task<PaginatedListDto<TResult>> GetPaginatedListAsync<TSource, TResult>(IQueryable<TSource> source, SieveModel queryParams, CancellationToken cancellationToken = default) where TResult : class
        {
            source = SieveProcessor.Apply(queryParams, source, applyPagination: false);

            return await source.ProjectToType<TResult>()
                .ToPaginatedListAsync(queryParams.Page, queryParams.PageSize, cancellationToken: cancellationToken);
        }

        protected IQueryable<TSource> ApplyQueryFilters<TSource>(IQueryable<TSource> source, SieveModel queryParams)
        {
            return SieveProcessor.Apply(queryParams, source, applyPagination: false);
        }
    }
}