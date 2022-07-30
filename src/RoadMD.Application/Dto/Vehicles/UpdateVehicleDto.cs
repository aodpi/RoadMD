namespace RoadMD.Application.Dto.Vehicles
{
    public class UpdateVehicleDto : CreateVehicleDto
    {
        public Guid Id { get; init; }
    }
}