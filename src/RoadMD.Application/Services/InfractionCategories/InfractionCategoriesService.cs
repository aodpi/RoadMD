using LanguageExt;
using LanguageExt.Common;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoadMD.Application.Dto.Common;
using RoadMD.Application.Dto.InfractionCategories;
using RoadMD.Application.Exceptions;
using RoadMD.Domain.Entities;
using RoadMD.EntityFrameworkCore;
using Sieve.Services;

namespace RoadMD.Application.Services.InfractionCategories
{
    public class InfractionCategoriesService : ServiceBase, IInfractionCategoriesService
    {
        private readonly ILogger<InfractionCategoriesService> _logger;

        public InfractionCategoriesService(ApplicationDbContext context, IMapper mapper,
            ILogger<InfractionCategoriesService> logger, ISieveProcessor sieveProcessor) : base(context, mapper, sieveProcessor)
        {
            _logger = logger;
        }

        public async Task<Result<InfractionCategoryDto>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dto = await Context.InfractionCategories
                .Where(x => x.Id.Equals(id))
                .ProjectToType<InfractionCategoryDto>()
                .SingleOrDefaultAsync(cancellationToken);

            return dto is null
                ? new Result<InfractionCategoryDto>(new NotFoundException(nameof(InfractionCategory), id))
                : new Result<InfractionCategoryDto>(dto);
        }

        public async Task<List<LookupDto>> GetSelectListAsync(CancellationToken cancellationToken = default)
        {
            return await Context.InfractionCategories
                .OrderBy(x => x.Name)
                .Select(x => new LookupDto(x.Id, x.Name))
                .ToListAsync(cancellationToken);
        }

        public async Task<Result<InfractionCategoryDto>> CreateAsync(CreateInfractionCategoryDto createInfractionCategory, CancellationToken cancellationToken = default)
        {
            var entity = new InfractionCategory
            {
                Name = createInfractionCategory.Name
            };
            await Context.InfractionCategories.AddAsync(entity, cancellationToken);

            try
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on create new infraction category");
                return new Result<InfractionCategoryDto>(e);
            }

            var dto = Mapper.Map<InfractionCategoryDto>(entity);

            return new Result<InfractionCategoryDto>(dto);
        }

        public async Task<Result<InfractionCategoryDto>> UpdateAsync(UpdateInfractionCategoryDto updateInfractionCategory, CancellationToken cancellationToken = default)
        {
            var entity = await Context.InfractionCategories
                .SingleOrDefaultAsync(x =>
                    x.Id.Equals(updateInfractionCategory.Id), cancellationToken: cancellationToken);

            if (entity is null)
            {
                return new Result<InfractionCategoryDto>(new NotFoundException(nameof(InfractionCategory), updateInfractionCategory.Id));
            }

            entity.Name = updateInfractionCategory.Name;

            Context.InfractionCategories.Update(entity);

            try
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on update infraction category \"{infractionCategoryId}\" ", updateInfractionCategory.Id);
                return new Result<InfractionCategoryDto>(e);
            }

            var dto = Mapper.Map<InfractionCategoryDto>(entity);

            return new Result<InfractionCategoryDto>(dto);
        }

        public async Task<Result<Unit>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await Context.InfractionCategories
                .SingleOrDefaultAsync(x =>
                    x.Id.Equals(id), cancellationToken: cancellationToken);

            if (entity is null)
            {
                return new Result<Unit>(new NotFoundException(nameof(InfractionCategory), id));
            }

            Context.InfractionCategories.Remove(entity);
            try
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on delete infraction category {infractionCategoryId}", id);
                return new Result<Unit>(e);
            }

            return new Result<Unit>(Unit.Default);
        }

        public async Task<Result<Unit>> BulkDeleteAsync(Guid[] ids, CancellationToken cancellationToken = default)
        {
            try
            {
                await Context.InfractionCategories
                    .Where(x => ids.Contains(x.Id))
                    .ExecuteDeleteAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on delete infraction categories: {InfractionCategoriesIds}", string.Join(',', ids));
                return new Result<Unit>(e);
            }

            return new Result<Unit>(Unit.Default);
        }
    }
}