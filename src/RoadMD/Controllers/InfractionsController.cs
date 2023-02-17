using Microsoft.AspNetCore.Mvc;
using RoadMD.Application.Dto.Common;
using RoadMD.Application.Dto.InfractionReports;
using RoadMD.Application.Dto.Infractions;
using RoadMD.Application.Dto.Infractions.Create;
using RoadMD.Application.Dto.Infractions.List;
using RoadMD.Application.Dto.Infractions.Update;
using RoadMD.Application.Services.InfractionReports;
using RoadMD.Application.Services.Infractions;
using RoadMD.Extensions;
using Sieve.Models;

namespace RoadMD.Controllers
{
    /// <summary>
    ///     Infractions
    /// </summary>
    [Route("api/infractions")]
    public class InfractionsController : ApiControllerBase
    {
        private readonly IInfractionReportService _infractionReportService;
        private readonly IInfractionService _infractionService;

        public InfractionsController(IInfractionService infractionService,
            IInfractionReportService infractionReportService)
        {
            _infractionService = infractionService;
            _infractionReportService = infractionReportService;
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

            return result.ToOk();
        }

        /// <summary>
        ///     List all infractions
        /// </summary>
        /// <param name="queryParams">Query Params</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedListDto<InfractionListDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromQuery] SieveModel queryParams,
            CancellationToken cancellationToken = default)
        {
            var result = await _infractionService.GetListAsync(queryParams, cancellationToken);
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
        [Consumes(typeof(CreateInfractionDto), "multipart/form-data", IsOptional = false)]
        public async Task<IActionResult> Create([FromForm] CreateInfractionDto input,
            CancellationToken cancellationToken = default)
        {
            var result = await _infractionService.CreateAsync(input, cancellationToken);

            return result.ToOk();
        }

        /// <summary>
        ///     Update infraction
        /// </summary>
        /// <param name="id">Infraction ID</param>
        /// <param name="input">Update infraction object</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateInfractionDto input,
            CancellationToken cancellationToken = default)
        {
            if (id != input.Id)
            {
                return BadRequest("Wrong ID");
            }

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

        /// <summary>
        ///     Create new infraction report
        /// </summary>
        /// <param name="id">Infraction ID</param>
        /// <param name="input">Create infraction report object</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("{id:guid}/report")]
        public async Task<IActionResult> AddReport([FromRoute] Guid id, [FromBody] CreateInfractionReportDto input,
            CancellationToken cancellationToken = default)
        {
            if (id != input.InfractionId)
            {
                return BadRequest("Wrong ID");
            }

            var result = await _infractionReportService.CreateAsync(input, cancellationToken);

            return result.ToOk();
        }

        /// <summary>
        ///     Update infraction report
        /// </summary>
        /// <param name="id">Infraction ID</param>
        /// <param name="reportId">Infraction report ID</param>
        /// <param name="input">Update infraction report object</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}/report/{reportId:guid}")]
        public async Task<IActionResult> UpdateReport([FromRoute] Guid id, [FromRoute] Guid reportId,
            [FromBody] UpdateInfractionReportDto input, CancellationToken cancellationToken = default)
        {
            if (id != input.InfractionId)
            {
                return BadRequest("Wrong ID");
            }

            if (reportId != input.Id)
            {
                return BadRequest("Wrong report ID");
            }

            var result = await _infractionReportService.UpdateAsync(input, cancellationToken);

            return result.ToNoContent();
        }

        /// <summary>
        ///     Delete infraction report
        /// </summary>
        /// <param name="id">Infraction ID</param>
        /// <param name="reportId">Infraction report ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}/report/{reportId:guid}")]
        public async Task<IActionResult> DeleteReport([FromRoute] Guid id, [FromRoute] Guid reportId,
            CancellationToken cancellationToken = default)
        {
            var result = await _infractionReportService.DeleteAsync(reportId, id, cancellationToken);

            return result.ToNoContent();
        }
    }
}