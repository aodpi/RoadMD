using Mapster;

namespace RoadMD.Application.Dto.Vehicle
{
    public class VehicleDto : IRegister
    {
        public Guid Id { get; init; }
        public string Number { get; init; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.Vehicle, VehicleDto>();
        }
    }
}