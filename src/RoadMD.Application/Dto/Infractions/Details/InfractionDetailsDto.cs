using Mapster;
using RoadMD.Domain.Entities;

namespace RoadMD.Application.Dto.Infractions.Details
{
    public class InfractionDetailsDto : IRegister
    {
        public InfractionDetailsDto()
        {
            Photos = Array.Empty<InfractionPhotoDto>();
        }

        public Guid Id { get; set; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public string? CategoryName { get; init; }

        public InfractionLocationDto? Location { get; init; }
        public InfractionVehicleDto? Vehicle { get; init; }

        public IEnumerable<InfractionPhotoDto> Photos { get; init; }


        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Infraction, InfractionDetailsDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.CategoryName, src => src.Category.Name)
                .Map(dest => dest.Location, src => src.Location)
                .Map(dest => dest.Vehicle, src => src.Vehicle)
                .Map(dest => dest.Photos, src => src.Photos)
                .IgnoreNonMapped(true);
        }
    }
}