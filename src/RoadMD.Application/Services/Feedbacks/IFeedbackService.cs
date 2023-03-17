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
        /// <summary>
        ///     Asynchronously retrieves a single FeedbackDto object based on the provided identifier.
        /// </summary>
        ///<param name="id">The unique identifier (GUID) of the desired Feedback object.</param>
        ///<param name="cancellationToken">An optional cancellation token to cancel the operation.</param>
        /// <returns>
        ///A Result object containing either a FeedbackDto object if found or a NotFoundException if the specified Feedback object is not found.
        ///</returns>
        Task<Result<FeedbackDto>> GetAsync(Guid id, CancellationToken cancellationToken = default);

        Task<PaginatedListDto<FeedbackGridDto>> GetListAsync(SieveModel queryModel, CancellationToken cancellationToken = default);
        Task<Result<FeedbackDto>> CreateAsync(CreateFeedbackDto input, CancellationToken cancellationToken = default);
        Task<Result<FeedbackDto>> UpdateAsync(UpdateFeedbackDto input, CancellationToken cancellationToken = default);
        Task<Result<Unit>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<Unit>> BulkDeleteAsync(Guid[] ids, CancellationToken cancellationToken = default);
    }
}