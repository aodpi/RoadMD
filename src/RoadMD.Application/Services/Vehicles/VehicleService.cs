using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using RoadMD.Application.Dto.Vehicle;
using RoadMD.Domain.Entities;
using RoadMD.EntityFrameworkCore;

namespace RoadMD.Application.Services.Vehicles
{
    public class VehicleService : ServiceBase, IVehicleService
    {
        public VehicleService(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<VehicleDto> Create(CreateVehicleDto input, CancellationToken cancellationToken = default)
        {
            var vehicle = new Vehicle
            {
                LetterCode = input.LetterCode,
                NumberCode = input.NumberCode
            };

            await Context.Vehicles.AddAsync(vehicle, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);

            return Mapper.Map<VehicleDto>(vehicle);
        }

        public async Task<IEnumerable<VehicleDto>> GetVehicles(CancellationToken cancellationToken = default)
        {
            var items = await Context.Vehicles.ProjectToType<VehicleDto>()
                .ToListAsync(cancellationToken);

            return items;
        }
    }
}
