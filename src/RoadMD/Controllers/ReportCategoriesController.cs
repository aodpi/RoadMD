using Microsoft.AspNetCore.Mvc;
using RoadMD.Application.Dto.Common;
using RoadMD.Application.Dto.ReportCategories;
using RoadMD.Application.Services.ReportCategories;
using RoadMD.Extensions;

namespace RoadMD.Controllers
{
    /// <summary>
    /// Report categories
    /// </summary>
    [Route("api/report-categories")]
    public class ReportCategoriesController : ApiControllerBase
    {
        private readonly IReportCategoryService _reportCategoryService;

        public ReportCategoriesController(IReportCategoryService reportCategoryService)
        {
            _reportCategoryService = reportCategoryService;
        }

        /// <summary>
        ///     Get report category by id
        /// </summary>
        /// <param name="id">Report Category ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ReportCategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _reportCategoryService.GetAsync(id, cancellationToken);

            return result.ToOk(x => x);
        }

        /// <summary>
        ///     Select list all report categories
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("select-list")]
        [ProducesResponseType(typeof(IEnumerable<LookupDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSelectList(CancellationToken cancellationToken = default)
        {
            return Ok(await _reportCategoryService.GetSelectListAsync(cancellationToken));
        }

        /// <summary>
        ///     Create new report category
        /// </summary>
        /// <param name="input">Create report category object</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateReportCategoryDto input,
            CancellationToken cancellationToken = default)
        {
            var result = await _reportCategoryService.CreateAsync(input, cancellationToken);
            return result.ToOk(infractionCategoryDto => infractionCategoryDto);
        }

        /// <summary>
        ///     Update report category
        /// </summary>
        /// <param name="id">Report Category ID</param>
        /// <param name="input">Update report category object</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateReportCategoryDto input,
            CancellationToken cancellationToken = default)
        {
            if (id != input.Id) return BadRequest("Wrong Id");
            var result = await _reportCategoryService.UpdateAsync(input, cancellationToken);
            return result.ToNoContent();
        }

        /// <summary>
        ///     Delete report category
        /// </summary>
        /// <param name="id">Report category ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _reportCategoryService.DeleteAsync(id, cancellationToken);
            return result.ToNoContent();
        }
    }
}