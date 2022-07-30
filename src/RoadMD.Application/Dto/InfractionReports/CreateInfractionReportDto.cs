namespace RoadMD.Application.Dto.InfractionReports
{
    public class CreateInfractionReportDto
    {
        public string Description { get; init; }
        public Guid ReportCategoryId { get; init; }
        public Guid InfractionId { get; init; }
    }
}