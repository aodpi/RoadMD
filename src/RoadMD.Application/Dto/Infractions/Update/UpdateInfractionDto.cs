﻿namespace RoadMD.Application.Dto.Infractions.Update
{
    public class UpdateInfractionDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }

        public Guid CategoryId { get; init; }
        public UpdateInfractionLocationDto Location { get; init; }
        public UpdateInfractionVehicleDto Vehicle { get; init; }
    }
}