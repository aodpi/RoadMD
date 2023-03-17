using Microsoft.AspNetCore.Mvc;
using RoadMD.Application.Dto.Common;
using RoadMD.Application.Dto.Feedbacks;
using RoadMD.Application.Dto.Feedbacks.Grid;
using RoadMD.Application.Services.Feedbacks;
using RoadMD.Extensions;
using Sieve.Models;

namespace RoadMD.Controllers
{
    /// <summary>
    ///     Feedbacks Controller
    /// </summary>
    [Route("api/feedbacks")]
    public class FeedbacksController : ApiControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbacksController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        /// <summary>
        ///     Retrieves a feedback by its ID.
        /// </summary>
        /// <param name="id">The ID of the feedback to retrieve.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>An IActionResult containing a FeedbackDto object if the feedback is found, or an appropriate error response otherwise.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(FeedbackDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _feedbackService.GetAsync(id, cancellationToken);

            return result.ToOk();
        }

        /// <summary>
        ///     Retrieves a paginated list of feedbacks based on the provided query parameters.
        /// </summary>
        /// <param name="queryParams">The query parameters containing filtering, sorting, and pagination options.</param>
        /// <param name="cancellationToken">An optional CancellationToken to observe while waiting for the task to complete. The default value is CancellationToken.None.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedListDto<FeedbackGridDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromQuery] SieveModel queryParams,
            CancellationToken cancellationToken = default)
        {
            var result = await _feedbackService.GetListAsync(queryParams, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        ///     Create new feedback
        /// </summary>
        /// <param name="input">Create feedback object</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateFeedbackDto input,
            CancellationToken cancellationToken = default)
        {
            var result = await _feedbackService.CreateAsync(input, cancellationToken);

            return result.ToOk();
        }


        /// <summary>
        ///     Update feedback
        /// </summary>
        /// <param name="id">Feedback ID</param>
        /// <param name="input">Update feedback object</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateFeedbackDto input,
            CancellationToken cancellationToken = default)
        {
            if (id != input.Id)
            {
                return BadRequest("Wrong ID");
            }

            var result = await _feedbackService.UpdateAsync(input, cancellationToken);

            return result.ToNoContent();
        }

        /// <summary>
        ///     Delete feedback
        /// </summary>
        /// <param name="id">Feedback ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _feedbackService.DeleteAsync(id, cancellationToken);

            return result.ToNoContent();
        }
    }
}