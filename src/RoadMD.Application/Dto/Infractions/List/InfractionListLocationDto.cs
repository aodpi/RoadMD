using Mapster;
using RoadMD.Domain.Entities;

namespace RoadMD.Application.Dto.Infractions.List
{
    public class InfractionListLocationDto : IRegister
    {
        public float Latitude { get; init; }
        public float Longitude { get; init; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Location, InfractionListLocationDto>()
                .Map(dest => dest.Latitude, src => src.Latitude)
                .Map(dest => dest.Longitude, src => src.Longitude)
                .IgnoreNonMapped(true);
        }
    }
}