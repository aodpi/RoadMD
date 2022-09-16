using Mapster;
using RoadMD.Domain.Entities;

namespace RoadMD.Application.Dto.Infractions.List
{
    public class InfractionListVehicleDto : IRegister
    {
        public string Number { get; init; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Vehicle, InfractionListVehicleDto>()
                .Map(dest => dest.Number, src => src.Number)
                .IgnoreNonMapped(true);
        }
    }
}