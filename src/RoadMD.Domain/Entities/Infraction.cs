namespace RoadMD.Domain.Entities
{
    public class Infraction : BaseEntity
    {
        public Infraction()
        {
            Photos = new HashSet<Photo>();
            Reports = new HashSet<InfractionReport>();
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public Guid CategoryId { get; set; }
        public InfractionCategory Category { get; set; }

        public Location Location { get; set; }
        public Guid LocationId { get; set; }

        public Vehicle Vehicle { get; set; }
        public Guid VehicleId { get; set; }

        public ICollection<Photo> Photos { get; set; }
        public ICollection<InfractionReport> Reports { get; set; }
    }
}