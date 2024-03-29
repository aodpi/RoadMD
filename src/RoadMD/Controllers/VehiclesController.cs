﻿using Microsoft.AspNetCore.Mvc;
using RoadMD.Application.Dto.Vehicles;
using RoadMD.Application.Services.Vehicles;
using System.ComponentModel.DataAnnotations;
using RoadMD.Application.Dto.Common;
using RoadMD.Extensions;
using Sieve.Models;

namespace RoadMD.Controllers
{
    /// <summary>
    /// Vehicles
    /// </summary>
    [Route("api/vehicles")]
    public class VehiclesController : ApiControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }


        /// <summary>
        ///     Get vehicle by id
        /// </summary>
        /// <param name="id">Vehicle ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _vehicleService.GetAsync(id, cancellationToken);

            return result.ToOk(vehicle => vehicle);
        }

        /// <summary>
        ///     List all reported vehicles
        /// </summary>
        /// <param name="queryParams">Query Params</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedListDto<VehicleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetVehicles([FromQuery] SieveModel queryParams,
            CancellationToken cancellationToken = default)
        {
            return Ok(await _vehicleService.GetListAsync(queryParams, cancellationToken));
        }

        /// <summary>
        ///     Create a new vehicle
        /// </summary>
        /// <param name="input">Provided Data</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateVehicleDto input,
            CancellationToken cancellationToken = default)
        {
            var result = await _vehicleService.CreateAsync(input, cancellationToken);
            return result.ToOk(vehicle => vehicle);
        }

        /// <summary>
        ///     Update vehicle
        /// </summary>
        /// <param name="id">Vehicle ID</param>
        /// <param name="input">Provided Data</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateVehicleDto input,
            CancellationToken cancellationToken = default)
        {
            if (id != input.Id) return BadRequest("Wrong Id");
            var result = await _vehicleService.UpdateAsync(input, cancellationToken);

            return result.ToNoContent();
        }

        /// <summary>
        ///     Delete vehicle
        /// </summary>
        /// <param name="id">Vehicle ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _vehicleService.DeleteAsync(id, cancellationToken);
            return result.ToNoContent();
        }
    }
}