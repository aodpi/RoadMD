namespace RoadMD.Domain.Entities
{
    public class InfractionReport : BaseEntity
    {
        public Infraction Infraction { get; set; }
        public Guid InfractionId { get; set; }

        public ReportCategory ReportCategory { get; set; }
        public Guid ReportCategoryId { get; set; }

        public string Description { get; set; }
    }
}