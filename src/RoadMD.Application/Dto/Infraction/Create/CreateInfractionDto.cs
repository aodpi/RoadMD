namespace RoadMD.Application.Dto.Infraction.Create
{
    [Serializable]
    public class CreateInfractionDto
    {
        public string Name { get; init; }
        public string Description { get; init; }

        public Guid CategoryId { get; init; }

        public CreateInfractionLocationDto Location { get; init; }
        public CreateInfractionVehicleDto Vehicle { get; init; }
    }
}