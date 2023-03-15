namespace RoadMD.Domain.Entities
{
    public class Photo : BaseEntity
    {
        public string Name { get; set; } = null!;
        public Guid BlobName { get; set; }
        public string Url { get; set; } = null!;

        public Guid InfractionId { get; set; }
        public Infraction Infraction { get; set; } = null!;
    }
}