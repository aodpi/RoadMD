using RoadMD.Application.Dto.Vehicle;

namespace RoadMD.Application.Services.Vehicles
{
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleDto>> GetVehicles(CancellationToken cancellationToken = default);
        Task<VehicleDto> Create(CreateVehicleDto input, CancellationToken cancellationToken = default);
    }
}
