using LanguageExt;
using LanguageExt.Common;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoadMD.Application.Dto.Common;
using RoadMD.Application.Dto.Infractions;
using RoadMD.Application.Dto.Infractions.Create;
using RoadMD.Application.Dto.Infractions.List;
using RoadMD.Application.Dto.Infractions.Update;
using RoadMD.Application.Exceptions;
using RoadMD.Domain.Entities;
using RoadMD.EntityFrameworkCore;
using RoadMD.Module.PhotoStorage.Abstractions;
using Sieve.Models;
using Sieve.Services;

namespace RoadMD.Application.Services.Infractions
{
    public class InfractionService : ServiceBase, IInfractionService
    {
        private readonly ILogger<InfractionService> _logger;
        private readonly IPhotoStorageService _photoStorage;

        public InfractionService(ApplicationDbContext context, IMapper mapper, ILogger<InfractionService> logger,
            IPhotoStorageService photoStorage, ISieveProcessor sieveProcessor) :
            base(context, mapper, sieveProcessor)
        {
            _logger = logger;
            _photoStorage = photoStorage;
        }

        public async Task<Result<InfractionDto>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var infractionDto = await Context.Infractions
                .Where(x => x.Id.Equals(id))
                .ProjectToType<InfractionDto>(Mapper.Config)
                .SingleOrDefaultAsync(cancellationToken);

            return infractionDto is null
                ? new Result<InfractionDto>(new NotFoundException(nameof(Infraction), id))
                : new Result<InfractionDto>(infractionDto);
        }

        public async Task<PaginatedListDto<InfractionListDto>> GetListAsync(SieveModel queryParams, CancellationToken cancellationToken = default)
        {
            var infractionQueryable = Context.Infractions
                .OrderBy(x => x.Name)
                .AsNoTracking();

            return await GetPaginatedListAsync<Infraction, InfractionListDto>(infractionQueryable, queryParams,
                cancellationToken);
        }

        public async Task<Result<InfractionDto>> CreateAsync(CreateInfractionDto input, CancellationToken cancellationToken = default)
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
                    Latitude = input.Location.Latitude
                }
            };

            if (vehicle is null)
            {
                infraction.Vehicle = new Vehicle
                {
                    Number = input.Vehicle.Number
                };
            }
            else
            {
                infraction.VehicleId = vehicle.Id;
            }

            if (input.Photos is not null && input.Photos.Any())
            {
                foreach (var photo in input.Photos)
                {
                    await using var stream = photo.OpenReadStream();

                    var (url, blobName) = await _photoStorage.StorePhoto(photo.FileName, stream, cancellationToken);

                    infraction.Photos.Add(new Photo
                    {
                        BlobName = blobName,
                        Name = photo.FileName,
                        Url = url
                    });
                }
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

        public async Task<Result<InfractionDto>> UpdateAsync(UpdateInfractionDto input, CancellationToken cancellationToken = default)
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
                if (infraction.Photos.Any())
                {
                    var blobNames = infraction.Photos.Select(f => f.BlobName);

                    await _photoStorage.DeletePhotos(blobNames, cancellationToken);
                }

                await Context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on delete infraction \"{InfractionId}\"", id);
                return new Result<Unit>(e);
            }

            return new Result<Unit>(Unit.Default);
        }

        public async Task<Result<Unit>> DeletePhotoAsync(Guid id, Guid photoId, CancellationToken cancellationToken = default)
        {
            var photo = await Context.Photos
                .Where(x => x.InfractionId.Equals(id) && x.Id.Equals(photoId))
                .SingleOrDefaultAsync(cancellationToken);

            if (photo is null)
            {
                return new Result<Unit>(new NotFoundException(nameof(Photo), id));
            }

            await using var transaction = await Context.Database.BeginTransactionAsync(cancellationToken);
            {
                Context.Photos.Remove(photo);

                try
                {
                    await Context.SaveChangesAsync(cancellationToken);
                    await _photoStorage.DeletePhotos(new[] { photo.BlobName }, cancellationToken);

                    await transaction.CommitAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error on delete photo \"{PhotoId}\"", photoId);
                    return new Result<Unit>(e);
                }
            }

            return new Result<Unit>();
        }
    }
}