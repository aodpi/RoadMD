using Microsoft.AspNetCore.Mvc;
using RoadMD.Application.Dto.Vehicle;
using RoadMD.Application.Services.Vehicles;
using System.ComponentModel.DataAnnotations;

namespace RoadMD.Controllers
{
    public class VehiclesController : ApiControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        /// <summary>
        /// List all reported vehicles
        /// </summary>
        /// <param name="letterCode">Filter by letter code</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VehicleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IEnumerable<VehicleDto>> GetVehicles([Required] string letterCode, CancellationToken cancellationToken = default)
        {
            return await _vehicleService.GetVehicles(cancellationToken);
        }

        /// <summary>
        /// Create a new vehicle
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<VehicleDto> CreateVehicle([FromBody] CreateVehicleDto input, CancellationToken cancellationToken = default)
        {
            return await _vehicleService.Create(input, cancellationToken);
        }
    }
}
