using Mapster;
using RoadMD.Domain.Entities;

namespace RoadMD.Application.Dto.Infractions
{
    public class InfractionDto : IRegister
    {
        public InfractionDto()
        {
            Photos = Array.Empty<InfractionPhotoDto>();
        }

        public Guid Id { get; set; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public Guid CategoryId { get; init; }

        public InfractionLocationDto? Location { get; init; }
        public InfractionVehicleDto? Vehicle { get; init; }

        public IEnumerable<InfractionPhotoDto> Photos { get; init; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Infraction, InfractionDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.CategoryId, src => src.CategoryId)
                .Map(dest => dest.Location, src => src.Location)
                .Map(dest => dest.Vehicle, src => src.Vehicle)
                .Map(dest => dest.Photos, src => src.Photos)
                .IgnoreNonMapped(true);
        }
    }
}