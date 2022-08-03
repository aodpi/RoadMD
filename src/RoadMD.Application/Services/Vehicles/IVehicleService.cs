using LanguageExt;
using LanguageExt.Common;
using RoadMD.Application.Dto.Common;
using RoadMD.Application.Dto.Vehicles;
using Sieve.Models;

namespace RoadMD.Application.Services.Vehicles
{
    public interface IVehicleService
    {
        Task<Result<VehicleDto>> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PaginatedListDto<VehicleDto>> GetListAsync(SieveModel queryModel, CancellationToken cancellationToken = default);
        Task<Result<VehicleDto>> CreateAsync(CreateVehicleDto input, CancellationToken cancellationToken = default);
        Task<Result<VehicleDto>> UpdateAsync(UpdateVehicleDto input, CancellationToken cancellationToken = default);
        Task<Result<Unit>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}