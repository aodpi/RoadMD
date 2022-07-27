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
using RoadMD.Modules.Abstractions;

namespace RoadMD.Application.Services.Infractions
{
    public class InfractionService : ServiceBase, IInfractionService
    {
        private readonly ILogger<InfractionService> _logger;
        private readonly IPhotoStorageService _photoStorage;

        public InfractionService(ApplicationDbContext context, IMapper mapper, ILogger<InfractionService> logger, IPhotoStorageService photoStorage) :
            base(context, mapper)
        {
            _logger = logger;
            _photoStorage = photoStorage;
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
                .SingleOrDefaultAsync(x => x.Number.Equals(input.Vehicle.Number), cancellationToken: cancellationToken);

            var infraction = new Infraction
            {
                Name = input.Name,
                Description = input.Description,
                CategoryId = input.CategoryId,
                Location = new Location
                {
                    Longitude = input.Location.Longitude,
                    Latitude = input.Location.Latitude,
                },
            };

            if (vehicle is null)
            {
                infraction.Vehicle = new Vehicle
                {
                    Number = input.Vehicle.Number,
                };
            }
            else
            {
                infraction.VehicleId = vehicle.Id;
            }

            foreach (var photo in input.Photos)
            {
                using var stream = photo.OpenReadStream();

                var (Url, BlobName) = await _photoStorage.StorePhoto(photo.FileName, stream, cancellationToken);

                infraction.Photos.Add(new Photo
                {
                    BlobName = BlobName,
                    Name = photo.FileName,
                    Url = Url,
                });
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
                .Include(x => x.Vehicle)
                .Include(x => x.Location)
                .SingleOrDefaultAsync(x => x.Id.Equals(input.Id), cancellationToken: cancellationToken);

            if (infraction is null)
                return new Result<InfractionDto>(new NotFoundException(nameof(Infraction), input.Id));

            infraction.Name = input.Name;
            infraction.Description = input.Description;
            infraction.CategoryId = input.CategoryId;

            infraction.Location.Latitude = input.Location.Latitude;
            infraction.Location.Longitude = input.Location.Longitude;

            if (!infraction.Vehicle.Number.Equals(input.Vehicle.Number))
            {
                var vehicle = await Context.Vehicles
                    .SingleOrDefaultAsync(x => x.Number.Equals(input.Vehicle.Number),
                        cancellationToken: cancellationToken) ?? new Vehicle
                        {
                            Number = input.Vehicle.Number
                        };

                infraction.Vehicle = vehicle;
            }

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
                .Include(f => f.Photos)
                .SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

            if (infraction is null)
            {
                return new Result<Unit>(new NotFoundException(nameof(Infraction), id));
            }

            Context.Infractions.Remove(infraction);

            try
            {
                foreach (var photo in infraction.Photos)
                {
                    await _photoStorage.DeletePhoto(photo.BlobName, cancellationToken);
                }
                await Context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on delete infraction \"{InfractionId}\"", id);
                return new Result<Unit>(e);
            }

            return new Result<Unit>();
        }

        public async Task<Result<Unit>> DeletePhotoAsync(Guid id, Guid photoId,
            CancellationToken cancellationToken = default)
        {
            var photo = await Context.Photos
                .Where(x => x.InfractionId.Equals(id) && x.Id.Equals(photoId))
                .SingleOrDefaultAsync(cancellationToken);

            if (photo is null)
            {
                return new Result<Unit>(new NotFoundException(nameof(Photo), id));
            }

            Context.Photos.Remove(photo);

            try
            {
                await Context.SaveChangesAsync(cancellationToken);
                await _photoStorage.DeletePhoto(photo.BlobName, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on delete photo \"{PhotoId}\"", photoId);
                return new Result<Unit>(e);
            }

            return new Result<Unit>();
        }
    }
}