using LanguageExt;
using LanguageExt.Common;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoadMD.Application.Dto.Common;
using RoadMD.Application.Dto.ReportCategories;
using RoadMD.Application.Exceptions;
using RoadMD.Domain.Entities;
using RoadMD.EntityFrameworkCore;
using Sieve.Services;

namespace RoadMD.Application.Services.ReportCategories
{
    public class ReportCategoryService : ServiceBase, IReportCategoryService
    {
        private readonly ILogger<ReportCategoryService> _logger;

        public ReportCategoryService(ApplicationDbContext context, IMapper mapper,
            ILogger<ReportCategoryService> logger, ISieveProcessor sieveProcessor) : base(context, mapper, sieveProcessor)
        {
            _logger = logger;
        }

        public async Task<Result<ReportCategoryDto>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dto = await Context.ReportCategories
                .AsNoTracking()
                .Where(x => x.Id.Equals(id))
                .ProjectToType<ReportCategoryDto>()
                .SingleOrDefaultAsync(cancellationToken);

            return dto is null
                ? new Result<ReportCategoryDto>(new NotFoundException(nameof(ReportCategory), id))
                : new Result<ReportCategoryDto>(dto);
        }

        public async Task<IEnumerable<LookupDto>> GetSelectListAsync(CancellationToken cancellationToken = default)
        {
            return await Context.ReportCategories
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new LookupDto(x.Id, x.Name))
                .ToListAsync(cancellationToken);
        }

        public async Task<Result<ReportCategoryDto>> CreateAsync(CreateReportCategoryDto input, CancellationToken cancellationToken = default)
        {
            var entity = new ReportCategory
            {
                Name = input.Name
            };

            await Context.ReportCategories.AddAsync(entity, cancellationToken);

            try
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on create new report category");
                return new Result<ReportCategoryDto>(e);
            }


            var dto = Mapper.Map<ReportCategoryDto>(entity);

            return new Result<ReportCategoryDto>(dto);
        }

        public async Task<Result<ReportCategoryDto>> UpdateAsync(UpdateReportCategoryDto input, CancellationToken cancellationToken = default)
        {
            var reportCategory = await Context.ReportCategories
                .SingleOrDefaultAsync(x => x.Id.Equals(input.Id), cancellationToken);

            if (reportCategory is null)
                return new Result<ReportCategoryDto>(new NotFoundException(nameof(ReportCategory), input.Id));

            reportCategory.Name = input.Name;

            Context.ReportCategories.Update(reportCategory);

            try
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on update report category \"{ReportCategoryId}\"", input.Id);
                return new Result<ReportCategoryDto>(e);
            }

            var dto = Mapper.Map<ReportCategoryDto>(reportCategory);

            return new Result<ReportCategoryDto>(dto);
        }

        public async Task<Result<Unit>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var reportCategory = await Context.ReportCategories
                .SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

            if (reportCategory is null)
                return new Result<Unit>(new NotFoundException(nameof(ReportCategory), id));

            Context.ReportCategories.Remove(reportCategory);

            try
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on delete report category \"{ReportCategoryId}\"", id);
                return new Result<Unit>(e);
            }

            return new Result<Unit>(Unit.Default);
        }
    }
}