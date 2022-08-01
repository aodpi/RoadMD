using Microsoft.AspNetCore.Http;

namespace RoadMD.Application.Dto.Infractions.Create
{
    public class CreateInfractionDto
    {
        public string Name { get; init; }
        public string Description { get; init; }

        public Guid CategoryId { get; init; }

        public CreateInfractionLocationDto Location { get; init; }
        public CreateInfractionVehicleDto Vehicle { get; init; }
        public IFormFileCollection? Photos { get; init; }
    }
}