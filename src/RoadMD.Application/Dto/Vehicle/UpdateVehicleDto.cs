namespace RoadMD.Application.Dto.Vehicle
{
    public class UpdateVehicleDto : CreateVehicleDto
    {
        public Guid Id { get; init; }
    }
}