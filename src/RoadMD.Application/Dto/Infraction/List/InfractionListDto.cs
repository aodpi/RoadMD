namespace RoadMD.Application.Dto.Infraction.List
{
    public class InfractionListDto
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public string? CategoryName { get; init; }

        public InfractionListLocationDto? Location { get; init; }
        public InfractionListVehicleDto? Vehicle { get; init; }
    }
}