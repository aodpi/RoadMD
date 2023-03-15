namespace RoadMD.Domain.Entities
{
    public class InfractionReport : BaseEntity
    {
        public Infraction Infraction { get; set; } = null!;
        public Guid InfractionId { get; set; }

        public ReportCategory ReportCategory { get; set; } = null!;
        public Guid ReportCategoryId { get; set; }

        public string? Description { get; set; }
    }
}