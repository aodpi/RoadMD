namespace RoadMD.Domain.Entities
{
    public class ReportCategory : BaseEntity
    {
        public ReportCategory()
        {
            InfractionReports = new HashSet<InfractionReport>();
        }

        public string Name { get; set; }

        public ICollection<InfractionReport> InfractionReports { get; set; }
    }
}