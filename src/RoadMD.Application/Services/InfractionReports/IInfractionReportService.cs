using LanguageExt;
using LanguageExt.Common;
using RoadMD.Application.Dto.Common;
using RoadMD.Application.Dto.InfractionReports;
using Sieve.Models;

namespace RoadMD.Application.Services.InfractionReports
{
    public interface IInfractionReportService
    {
        Task<Result<InfractionReportDto>> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PaginatedListDto<InfractionReportDto>> GetListAsync(SieveModel queryModel, CancellationToken cancellationToken = default);
        Task<Result<InfractionReportDto>> CreateAsync(CreateInfractionReportDto input, CancellationToken cancellationToken = default);
        Task<Result<InfractionReportDto>> UpdateAsync(UpdateInfractionReportDto input, CancellationToken cancellationToken = default);
        Task<Result<Unit>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}