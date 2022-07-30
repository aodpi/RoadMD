using LanguageExt;
using LanguageExt.Common;
using RoadMD.Application.Dto.Common;
using RoadMD.Application.Dto.ReportCategories;

namespace RoadMD.Application.Services.ReportCategories
{
    public interface IReportCategoryService
    {
        Task<Result<ReportCategoryDto>> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<LookupDto>> GetSelectListAsync(CancellationToken cancellationToken = default);

        Task<Result<ReportCategoryDto>> CreateAsync(CreateReportCategoryDto input,
            CancellationToken cancellationToken = default);

        Task<Result<ReportCategoryDto>> UpdateAsync(UpdateReportCategoryDto input,
            CancellationToken cancellationToken = default);

        Task<Result<Unit>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}