namespace RoadMD.Domain.Entities
{
    public class Photo : BaseEntity
    {
        public string Name { get; set; }
        public Guid BlobName { get; set; }
        public string Url { get; set; }

        public Guid InfractionId { get; set; }
        public Infraction Infraction { get; set; }
    }
}