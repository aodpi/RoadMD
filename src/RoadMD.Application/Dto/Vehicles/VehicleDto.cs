using Mapster;
using RoadMD.Domain.Entities;

namespace RoadMD.Application.Dto.Vehicles
{
    public class VehicleDto : IRegister
    {
        public Guid Id { get; init; }
        public string Number { get; init; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Vehicle, VehicleDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Number, src => src.Number)
                .IgnoreNonMapped(true);
        }
    }
}