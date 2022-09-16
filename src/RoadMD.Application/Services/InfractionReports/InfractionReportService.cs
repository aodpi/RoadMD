using LanguageExt;
using LanguageExt.Common;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoadMD.Application.Dto.Common;
using RoadMD.Application.Dto.InfractionReports;
using RoadMD.Application.Exceptions;
using RoadMD.Domain.Entities;
using RoadMD.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace RoadMD.Application.Services.InfractionReports
{
    public class InfractionReportService : ServiceBase, IInfractionReportService
    {
        private readonly ILogger<InfractionReportService> _logger;

        /// <inheritdoc />
        public InfractionReportService(ApplicationDbContext context, IMapper mapper,
            ILogger<InfractionReportService> logger, ISieveProcessor sieveProcessor) : base(context, mapper,
            sieveProcessor)
        {
            _logger = logger;
        }

        public async Task<Result<InfractionReportDto>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var infractionReportDto = await Context.InfractionReports
                .Where(x => x.Id.Equals(id))
                .ProjectToType<InfractionReportDto>()
                .SingleOrDefaultAsync(cancellationToken);

            return infractionReportDto is null
                ? new Result<InfractionReportDto>(new NotFoundException(nameof(InfractionReport), id))
                : new Result<InfractionReportDto>(infractionReportDto);
        }

        public async Task<PaginatedListDto<InfractionReportDto>> GetListAsync(SieveModel queryModel, CancellationToken cancellationToken = default)
        {
            return await GetPaginatedListAsync<InfractionReport, InfractionReportDto>(
                Context.InfractionReports.AsNoTracking(), queryModel, cancellationToken);
        }

        public async Task<Result<InfractionReportDto>> CreateAsync(CreateInfractionReportDto input, CancellationToken cancellationToken = default)
        {
            var infractionReport = new InfractionReport
            {
                Description = input.Description,
                ReportCategoryId = input.ReportCategoryId,
                InfractionId = input.InfractionId
            };

            await Context.InfractionReports.AddAsync(infractionReport, cancellationToken);


            try
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on save new infraction report");
                return new Result<InfractionReportDto>(e);
            }

            var infractionReportDto = Mapper.Map<InfractionReportDto>(infractionReport);

            return new Result<InfractionReportDto>(infractionReportDto);
        }

        public async Task<Result<InfractionReportDto>> UpdateAsync(UpdateInfractionReportDto input, CancellationToken cancellationToken = default)
        {
            var infractionReport = await Context.InfractionReports
                .Where(x => x.Id.Equals(input.Id))
                .SingleOrDefaultAsync(cancellationToken);

            if (infractionReport is null)
                return new Result<InfractionReportDto>(new NotFoundException(nameof(InfractionReport), input.Id));

            infractionReport.ReportCategoryId = input.ReportCategoryId;
            infractionReport.Description = input.Description;
            infractionReport.InfractionId = input.InfractionId;

            Context.InfractionReports.Update(infractionReport);

            try
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on update infraction report \"{InfractionReportId}\"", input.Id);

                return new Result<InfractionReportDto>(e);
            }

            var infractionReportDto = Mapper.Map<InfractionReportDto>(infractionReport);

            return new Result<InfractionReportDto>(infractionReportDto);
        }

        public async Task<Result<Unit>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var infractionReport = await Context.InfractionReports
                .Where(x => x.Id.Equals(id))
                .SingleOrDefaultAsync(cancellationToken);

            if (infractionReport is null)
                return new Result<Unit>(new NotFoundException(nameof(InfractionReport), id));

            Context.InfractionReports.Remove(infractionReport);

            try
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on delete infraction report \"{InfractionReportId}\"", id);
                return new Result<Unit>(e);
            }

            return new Result<Unit>(Unit.Default);
        }
    }
}