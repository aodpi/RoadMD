namespace RoadMD.Application.Dto.Infraction.Create
{
    public class CreateInfractionDto
    {
        public CreateInfractionDto()
        {
            Photos = Array.Empty<CreateInfractionPhotoDto>();
        }

        public string Name { get; init; }
        public string Description { get; init; }

        public Guid CategoryId { get; init; }

        public CreateInfractionLocationDto Location { get; init; }
        public CreateInfractionVehicleDto Vehicle { get; init; }
        public IEnumerable<CreateInfractionPhotoDto> Photos { get; init; }
    }
}