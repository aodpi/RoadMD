using Microsoft.AspNetCore.Mvc;
using RoadMD.Application.Dto.Common;
using RoadMD.Application.Dto.Feedbacks;
using RoadMD.Application.Dto.InfractionCategories;
using RoadMD.Application.Dto.Infractions.Create;
using RoadMD.Application.Dto.Infractions.List;
using RoadMD.Application.Dto.Infractions.Update;
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
        ///     Get feedback by id
        /// </summary>
        /// <param name="id">Feedback ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
        ///     List all feedbacks
        /// </summary>
        /// <param name="queryParams">Query Params</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedListDto<FeedbackDto>), StatusCodes.Status200OK)]
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
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateFeedbackDto input,
            CancellationToken cancellationToken = default)
        {
            if (id != input.Id) return BadRequest("Wrong ID");
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