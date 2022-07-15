namespace RoadMD.Domain.Entities
{
    public class Infraction : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Guid CategoryId { get; set; }
        public InfractionCategory Category { get; set; }

        public Location Location { get; set; }
        public Guid LocationId { get; set; }

        public Vehicle Vehicle { get; set; }
        public Guid VehicleId { get; set; }
    }
}