using LanguageExt;
using LanguageExt.Common;
using RoadMD.Application.Dto.InfractionReports;

namespace RoadMD.Application.Services.InfractionReports
{
    public interface IInfractionReportService
    {
        Task<Result<InfractionReportDto>> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<InfractionReportDto>> GetListAsync(CancellationToken cancellationToken = default);
        Task<Result<InfractionReportDto>> CreateAsync(CreateInfractionReportDto input, CancellationToken cancellationToken = default);
        Task<Result<InfractionReportDto>> UpdateAsync(UpdateInfractionReportDto input, CancellationToken cancellationToken = default);
        Task<Result<Unit>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}