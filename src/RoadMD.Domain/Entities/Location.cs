namespace RoadMD.Domain.Entities
{
    public class Location : BaseEntity
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public Infraction Infraction { get; set; }
        public Guid InfractionId { get; set; }
    }
}