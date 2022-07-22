using Microsoft.AspNetCore.Mvc;
using RoadMD.Application.Dto.Infraction;
using RoadMD.Application.Dto.Infraction.Create;
using RoadMD.Application.Dto.Infraction.List;
using RoadMD.Application.Dto.Infraction.Update;
using RoadMD.Application.Services.Infractions;
using RoadMD.Extensions;

namespace RoadMD.Controllers
{
    [Route("api/infractions")]
    public class InfractionsController : ApiControllerBase
    {
        private readonly IInfractionService _infractionService;

        public InfractionsController(IInfractionService infractionService)
        {
            _infractionService = infractionService;
        }

        /// <summary>
        ///     Get infraction by id
        /// </summary>
        /// <param name="id">Infraction ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(InfractionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _infractionService.GetAsync(id, cancellationToken);

            return result.ToOk(x => x);
        }

        /// <summary>
        ///     List all infractions
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InfractionListDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList(CancellationToken cancellationToken = default)
        {
            var result = await _infractionService.GetListAsync(cancellationToken);
            return Ok(result);
        }

        /// <summary>
        ///     Create new infraction
        /// </summary>
        /// <param name="input">Create infraction object</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateInfractionDto input,
            CancellationToken cancellationToken = default)
        {
            var result = await _infractionService.CreateAsync(input, cancellationToken);

            return result.ToOk(x => x);
        }

        /// <summary>
        ///     Update infraction
        /// </summary>
        /// <param name="id">Infraction ID</param>
        /// <param name="input">Update infraction object</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateInfractionDto input,
            CancellationToken cancellationToken = default)
        {
            if (id != input.Id) return BadRequest("Wrong ID");
            var result = await _infractionService.UpdateAsync(input, cancellationToken);

            return result.ToNoContent();
        }

        /// <summary>
        ///     Delete infraction
        /// </summary>
        /// <param name="id">Infraction ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _infractionService.DeleteAsync(id, cancellationToken);

            return result.ToNoContent();
        }

        /// <summary>
        ///     Delete infraction photo
        /// </summary>
        /// <param name="id">Infraction ID</param>
        /// <param name="photoId">Photo Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}/photo/{photoId:guid}")]
        public async Task<IActionResult> DeletePhoto([FromRoute] Guid id, [FromRoute] Guid photoId,
            CancellationToken cancellationToken = default)
        {
            var result = await _infractionService.DeletePhotoAsync(id, photoId, cancellationToken);

            return result.ToNoContent();
        }
    }
}