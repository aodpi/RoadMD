using LanguageExt;
using LanguageExt.Common;
using RoadMD.Application.Dto.Common;
using RoadMD.Application.Dto.Feedbacks;
using RoadMD.Application.Dto.Feedbacks.Grid;
using Sieve.Models;

namespace RoadMD.Application.Services.Feedbacks
{
    public interface IFeedbackService
    {
        Task<Result<FeedbackDto>> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PaginatedListDto<FeedbackGridDto>> GetListAsync(SieveModel queryModel, CancellationToken cancellationToken = default);
        Task<Result<FeedbackDto>> CreateAsync(CreateFeedbackDto input, CancellationToken cancellationToken = default);
        Task<Result<FeedbackDto>> UpdateAsync(UpdateFeedbackDto input, CancellationToken cancellationToken = default);
        Task<Result<Unit>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<Unit>> BulkDeleteAsync(Guid[] ids, CancellationToken cancellationToken = default);
    }
}