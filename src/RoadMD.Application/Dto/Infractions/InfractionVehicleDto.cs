using Mapster;
using RoadMD.Domain.Entities;

namespace RoadMD.Application.Dto.Infractions
{
    public class InfractionVehicleDto : IRegister
    {
        public string Number { get; init; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Vehicle, InfractionVehicleDto>()
                .Map(dest => dest.Number, src => src.Number)
                .IgnoreNonMapped(true);
        }
    }
}