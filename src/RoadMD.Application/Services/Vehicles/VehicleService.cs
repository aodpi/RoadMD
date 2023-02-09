using LanguageExt;
using LanguageExt.Common;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoadMD.Application.Dto.Common;
using RoadMD.Application.Dto.Vehicles;
using RoadMD.Application.Exceptions;
using RoadMD.Domain.Entities;
using RoadMD.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace RoadMD.Application.Services.Vehicles
{
    public class VehicleService : ServiceBase, IVehicleService
    {
        private readonly ILogger<VehicleService> _logger;

        public VehicleService(ApplicationDbContext context, IMapper mapper, ILogger<VehicleService> logger,
            ISieveProcessor sieveProcessor) : base(context, mapper, sieveProcessor)
        {
            _logger = logger;
        }

        public async Task<Result<VehicleDto>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await Context.Vehicles
                .AsNoTracking()
                .Where(x => x.Id.Equals(id))
                .ProjectToType<VehicleDto>()
                .SingleOrDefaultAsync(cancellationToken);

            return entity is null
                ? new Result<VehicleDto>(new NotFoundException(nameof(Vehicle), id))
                : new Result<VehicleDto>(entity);
        }

        public async Task<PaginatedListDto<VehicleDto>> GetListAsync(SieveModel queryModel, CancellationToken cancellationToken = default)
        {
            return await GetPaginatedListAsync<Vehicle, VehicleDto>(Context.Vehicles.AsNoTracking(), queryModel,
                cancellationToken);
        }

        public async Task<Result<VehicleDto>> CreateAsync(CreateVehicleDto input, CancellationToken cancellationToken = default)
        {
            var vehicle = new Vehicle
            {
                Number = input.Number
            };

            await Context.Vehicles.AddAsync(vehicle, cancellationToken);

            try
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on save new vehicle");
                return new Result<VehicleDto>(e);
            }

            await Context.SaveChangesAsync(cancellationToken);

            var dto = Mapper.Map<VehicleDto>(vehicle);
            return new Result<VehicleDto>(dto);
        }

        public async Task<Result<VehicleDto>> UpdateAsync(UpdateVehicleDto input, CancellationToken cancellationToken = default)
        {
            var entity = await Context.Vehicles
                .SingleOrDefaultAsync(x => x.Id.Equals(input.Id), cancellationToken);

            if (entity is null) return new Result<VehicleDto>(new NotFoundException(nameof(Vehicle), input.Id));

            entity.Number = input.Number;

            Context.Vehicles.Update(entity);

            try
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on update vehicle \"{VehicleId}\"", input.Id);
                return new Result<VehicleDto>(e);
            }

            var dto = Mapper.Map<VehicleDto>(entity);

            return new Result<VehicleDto>(dto);
        }

        public async Task<Result<Unit>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await Context.Vehicles
                .SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

            if (entity is null)
            {
                return new Result<Unit>(new NotFoundException(nameof(Vehicle), id));
            }

            Context.Vehicles.Remove(entity);

            try
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on delete vehicle \"{VehicleId}\"", id);
                return new Result<Unit>(e);
            }

            return new Result<Unit>(Unit.Default);
        }

        public async Task<Result<Unit>> BulkDeleteAsync(Guid[] ids, CancellationToken cancellationToken = default)
        {
            try
            {
                await Context.Vehicles
                    .Where(x => ids.Contains(x.Id))
                    .ExecuteDeleteAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on delete vehicle \"{VehicleIds}\"", string.Join(',', ids));
                return new Result<Unit>(e);
            }

            return new Result<Unit>(Unit.Default);
        }
    }
}