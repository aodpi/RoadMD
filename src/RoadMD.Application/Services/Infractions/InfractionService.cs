using LanguageExt;
using LanguageExt.Common;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoadMD.Application.Dto.Infraction;
using RoadMD.Application.Dto.Infraction.Create;
using RoadMD.Application.Dto.Infraction.List;
using RoadMD.Application.Dto.Infraction.Update;
using RoadMD.Application.Exceptions;
using RoadMD.Domain.Entities;
using RoadMD.EntityFrameworkCore;

namespace RoadMD.Application.Services.Infractions
{
    public class InfractionService : ServiceBase, IInfractionService
    {
        private readonly ILogger<InfractionService> _logger;

        public InfractionService(ApplicationDbContext context, IMapper mapper, ILogger<InfractionService> logger) :
            base(context, mapper)
        {
            _logger = logger;
        }

        public async Task<Result<InfractionDto>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var infractionDto = await Context.Infractions
                .Where(x => x.Id.Equals(id))
                .ProjectToType<InfractionDto>()
                .SingleOrDefaultAsync(cancellationToken);

            return infractionDto is null
                ? new Result<InfractionDto>(new NotFoundException(nameof(Infraction), id))
                : new Result<InfractionDto>(infractionDto);
        }

        public async Task<IEnumerable<InfractionListDto>> GetListAsync(
            CancellationToken cancellationToken = default)
        {
            return await Context.Infractions
                .OrderBy(x => x.Name)
                .ProjectToType<InfractionListDto>()
                .ToListAsync(cancellationToken);
        }

        public async Task<Result<InfractionDto>> CreateAsync(CreateInfractionDto input,
            CancellationToken cancellationToken = default)
        {
            var vehicle = await Context.Vehicles
                .SingleOrDefaultAsync(x =>
                        x.LetterCode.Equals(input.Vehicle.LetterCode) &&
                        x.NumberCode.Equals(input.Vehicle.NumberCode),
                    cancellationToken: cancellationToken);

            var infraction = new Infraction
            {
                Name = input.Name,
                Description = input.Description,
                CategoryId = input.CategoryId,
                Location = new Location
                {
                    Longitude = input.Location.Longitude,
                    Latitude = input.Location.Latitude,
                }
            };

            if (vehicle is null)
            {
                infraction.Vehicle = new Vehicle
                {
                    LetterCode = input.Vehicle.LetterCode,
                    NumberCode = input.Vehicle.NumberCode,
                };
            }
            else
            {
                infraction.VehicleId = vehicle.Id;
            }

            await Context.Infractions.AddAsync(infraction, cancellationToken);

            try
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on save new infraction");
                return new Result<InfractionDto>(e);
            }

            var infractionDto = Mapper.Map<InfractionDto>(infraction);
            return new Result<InfractionDto>(infractionDto);
        }

        public async Task<Result<InfractionDto>> UpdateAsync(UpdateInfractionDto input,
            CancellationToken cancellationToken = default)
        {
            var infraction = await Context.Infractions
                .Include(x => x.Location)
                .SingleOrDefaultAsync(x => x.Id.Equals(input.Id), cancellationToken: cancellationToken);

            if (infraction is null)
                return new Result<InfractionDto>(new NotFoundException(nameof(Infraction), input.Id));

            infraction.Name = input.Name;
            infraction.Description = input.Description;
            infraction.CategoryId = input.CategoryId;

            infraction.Location.Latitude = input.Location.Latitude;
            infraction.Location.Longitude = input.Location.Longitude;

            Context.Infractions.Update(infraction);

            try
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on update infraction \"{InfractionId}\"", input.Id);

                return new Result<InfractionDto>(e);
            }

            var dto = Mapper.Map<InfractionDto>(infraction);

            return new Result<InfractionDto>(dto);
        }

        public async Task<Result<Unit>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var infraction = await Context.Infractions
                .SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

            if (infraction is null)
            {
                return new Result<Unit>(new NotFoundException(nameof(Infraction), id));
            }

            Context.Infractions.Remove(infraction);

            try
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on delete infraction \"{InfractionId}\"", id);
                return new Result<Unit>(e);
            }

            return new Result<Unit>();
        }
    }
}