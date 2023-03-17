using System.Runtime.CompilerServices;
using LanguageExt;
using LanguageExt.Common;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoadMD.Application.Dto.Common;
using RoadMD.Application.Dto.Feedbacks;
using RoadMD.Application.Dto.Feedbacks.Grid;
using RoadMD.Application.Exceptions;
using RoadMD.Domain.Entities;
using RoadMD.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace RoadMD.Application.Services.Feedbacks
{
    /// <summary>
    /// Provides CRUD operations for Feedback objects.
    /// </summary>
    public class FeedbackService : ServiceBase, IFeedbackService
    {
        private readonly ILogger<FeedbackService> _logger;

        public FeedbackService(ApplicationDbContext context, IMapper mapper, ISieveProcessor sieveProcessor,
            ILogger<FeedbackService> logger) : base(
            context, mapper, sieveProcessor)
        {
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a FeedbackDto object asynchronously based on the provided Guid and an optional CancellationToken.
        /// </summary>
        /// <param name="id">The Guid representing the unique identifier of the feedback.</param>
        /// <param name="cancellationToken">An optional CancellationToken to observe while waiting for the task to complete. The default value is CancellationToken.None.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains a Result of FeedbackDto object.</returns>
        public async Task<Result<FeedbackDto>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entityDto = await Context.Feedbacks
                .Where(x => x.Id.Equals(id))
                .ProjectToType<FeedbackDto>(Mapper.Config)
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);

            return entityDto is null
                ? new Result<FeedbackDto>(new NotFoundException(nameof(Feedback), id))
                : new Result<FeedbackDto>(entityDto);
        }

        public async Task<PaginatedListDto<FeedbackGridDto>> GetListAsync(SieveModel queryModel, CancellationToken cancellationToken = default)
        {
            return await GetPaginatedListAsync<Feedback, FeedbackGridDto>(Context.Feedbacks.AsQueryable(), queryModel,
                cancellationToken);
        }

        /// <summary>
        /// Creates a new Feedback asynchronously based on the provided CreateFeedbackDto object and an optional CancellationToken.
        /// </summary>
        /// <param name="input">The CreateFeedbackDto object containing the data for the new feedback.</param>
        /// <param name="cancellationToken">An optional CancellationToken to observe while waiting for the task to complete. The default value is CancellationToken.None.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains a Result of FeedbackDto object.</returns>
        public async Task<Result<FeedbackDto>> CreateAsync(CreateFeedbackDto input, CancellationToken cancellationToken = default)
        {
            var entity = new Feedback
            {
                Subject = input.Subject,
                Description = input.Description,
                UserEmail = input.UserEmail,
                UserName = input.UserName
            };

            await Context.Feedbacks.AddAsync(entity, cancellationToken);

            try
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on create new feedback");
                return new Result<FeedbackDto>(e);
            }

            return new Result<FeedbackDto>(Mapper.Map<FeedbackDto>(entity));
        }

        /// <summary>
        /// Updates an existing Feedback asynchronously based on the provided UpdateFeedbackDto object and an optional CancellationToken.
        /// </summary>
        /// <param name="input">The UpdateFeedbackDto object containing the updated data for the feedback.</param>
        /// <param name="cancellationToken">An optional CancellationToken to observe while waiting for the task to complete. The default value is CancellationToken.None.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains a Result of FeedbackDto object.</returns>
        public async Task<Result<FeedbackDto>> UpdateAsync(UpdateFeedbackDto input, CancellationToken cancellationToken = default)
        {
            var feedbackDb = await Context.Feedbacks
                .SingleOrDefaultAsync(x => x.Id.Equals(input.Id), cancellationToken);

            if (feedbackDb is null)
            {
                return new Result<FeedbackDto>(new NotFoundException(nameof(Feedback), input.Id));
            }

            feedbackDb.Subject = input.Subject;
            feedbackDb.Description = input.Description;
            feedbackDb.UserEmail = input.UserEmail;
            feedbackDb.UserName = input.UserName;

            Context.Feedbacks.Update(feedbackDb);

            try
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on update feedback \"{FeedbackId}\" ", input.Id);

                return new Result<FeedbackDto>(e);
            }

            return new Result<FeedbackDto>(Mapper.Map<FeedbackDto>(feedbackDb));
        }

        public async Task<Result<Unit>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbEntity = await Context.Feedbacks.SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
            if (dbEntity is null)
            {
                return new Result<Unit>(new NotFoundException(nameof(Feedback), id));
            }

            Context.Feedbacks.Remove(dbEntity);

            try
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on delete feedback \"{FeedbackId}\"", id);
                return new Result<Unit>(e);
            }

            return new Result<Unit>(Unit.Default);
        }

        public async Task<Result<Unit>> BulkDeleteAsync(Guid[] ids, CancellationToken cancellationToken = default)
        {
            if (ids.Length == 0)
            {
                return new Result<Unit>(Unit.Default);
            }

            try
            {
                await Context.Feedbacks.Where(x => ids.Contains(x.Id))
                    .ExecuteDeleteAsync(cancellationToken: cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on bulk delete feedbacks. Ids: {FeedbackIds}", string.Join(", ", ids));
                return new Result<Unit>(e);
            }

            return new Result<Unit>(Unit.Default);
        }
    }
}