namespace RoadMD.Application.Dto.Infraction
{
    public class InfractionDto
    {
        public InfractionDto()
        {
            Photos = Array.Empty<InfractionPhotoDto>();
        }

        public Guid Id { get; set; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public Guid CategoryId { get; init; }

        public InfractionLocationDto? Location { get; init; }
        public InfractionVehicleDto? Vehicle { get; init; }

        public IEnumerable<InfractionPhotoDto> Photos { get; init; }
    }
}