using Mapster;

namespace RoadMD.Application.Dto.Infraction.List
{
    public class InfractionListDto : IRegister
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public string? CategoryName { get; init; }

        public InfractionListLocationDto? Location { get; init; }
        public InfractionListVehicleDto? Vehicle { get; init; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.Infraction, InfractionListDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.CategoryName, src => src.Category.Name)
                .Map(dest => dest.Location, src => new InfractionListLocationDto
                {
                    Longitude = src.Location.Longitude,
                    Latitude = src.Location.Latitude
                })
                .Map(dest => dest.Vehicle, src => new InfractionListVehicleDto
                {
                    Number = src.Vehicle.Number,
                })
                .IgnoreNonMapped(true);
        }
    }
}