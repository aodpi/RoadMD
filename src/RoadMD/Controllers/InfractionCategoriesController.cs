using Microsoft.AspNetCore.Mvc;
using RoadMD.Application.Dto.Common;
using RoadMD.Application.Dto.InfractionCategories;
using RoadMD.Application.Services.InfractionCategories;
using RoadMD.Extensions;

namespace RoadMD.Controllers
{
    /// <summary>
    ///     Infraction categories
    /// </summary>
    [Route("api/infraction-categories")]
    public class InfractionCategoriesController : ApiControllerBase
    {
        private readonly IInfractionCategoriesService _infractionCategoriesService;

        public InfractionCategoriesController(IInfractionCategoriesService infractionCategoriesService)
        {
            _infractionCategoriesService = infractionCategoriesService;
        }

        /// <summary>
        ///     Get infraction category by id
        /// </summary>
        /// <param name="id">Infraction Category ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(InfractionCategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _infractionCategoriesService.GetAsync(id, cancellationToken);

            return result.ToOk();
        }

        /// <summary>
        ///     Select list all infraction categories
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("select-list")]
        [ProducesResponseType(typeof(IEnumerable<LookupDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSelectList(CancellationToken cancellationToken = default)
        {
            return Ok(await _infractionCategoriesService.GetSelectListAsync(cancellationToken));
        }

        /// <summary>
        ///     Create new infraction category
        /// </summary>
        /// <param name="input">Create infraction category object</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateInfractionCategoryDto input,
            CancellationToken cancellationToken = default)
        {
            var result = await _infractionCategoriesService.CreateAsync(input, cancellationToken);
            return result.ToOk(infractionCategoryDto => infractionCategoryDto);
        }

        /// <summary>
        ///     Update infraction category
        /// </summary>
        /// <param name="id">Infraction Category ID</param>
        /// <param name="input">Update infraction category object</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateInfractionCategoryDto input,
            CancellationToken cancellationToken = default)
        {
            if (id != input.Id)
            {
                return BadRequest("Wrong Id");
            }

            var result = await _infractionCategoriesService.UpdateAsync(input, cancellationToken);
            return result.ToNoContent();
        }

        /// <summary>
        ///     Delete infraction category
        /// </summary>
        /// <param name="id">Infraction category ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _infractionCategoriesService.DeleteAsync(id, cancellationToken);
            return result.ToNoContent();
        }
    }
}