using LanguageExt;
using LanguageExt.Common;
using RoadMD.Application.Dto.Common;
using RoadMD.Application.Dto.InfractionReports;
using Sieve.Models;

namespace RoadMD.Application.Services.InfractionReports
{
    public interface IInfractionReportService
    {
        /// <summary>
        ///     Get infraction report by provided ID
        /// </summary>
        /// <param name="id">Infraction ID</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> controlling the request lifetime.</param>
        /// <returns></returns>
        Task<Result<InfractionReportDto>> GetAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get paginated list of infraction reports
        /// </summary>
        /// <param name="queryModel">Query model</param>
        /// <param name="cancellationToken">>A <see cref="CancellationToken"/> controlling the request lifetime.</param>
        /// <returns></returns>
        Task<PaginatedListDto<InfractionReportDto>> GetListAsync(SieveModel queryModel, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Create a new infraction report
        /// </summary>
        /// <param name="input">Infraction report object</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> controlling the request lifetime.</param>
        /// <returns></returns>
        Task<Result<InfractionReportDto>> CreateAsync(CreateInfractionReportDto input, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Update an infraction report
        /// </summary>
        /// <param name="input">Updated infraction report model</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> controlling the request lifetime.</param>
        /// <returns></returns>
        Task<Result<InfractionReportDto>> UpdateAsync(UpdateInfractionReportDto input, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Delete an infraction report by provided ID
        /// </summary>
        /// <param name="id">Infraction Report ID</param>
        /// <param name="infractionId">Infraction Id</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> controlling the request lifetime.</param>
        /// <returns></returns>
        Task<Result<Unit>> DeleteAsync(Guid id, Guid? infractionId = default, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Delete a range of infraction reports
        /// </summary>
        /// <param name="ids">Infraction Reports IDs</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> controlling the request lifetime.</param>
        /// <returns></returns>
        Task<Result<Unit>> BulkDeleteAsync(Guid[] ids, CancellationToken cancellationToken = default);
    }
}