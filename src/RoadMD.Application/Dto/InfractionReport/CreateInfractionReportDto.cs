namespace RoadMD.Application.Dto.InfractionReport
{
    public class CreateInfractionReportDto
    {
        public string Description { get; init; }
        public Guid ReportCategoryId { get; init; }
        public Guid InfractionId { get; init; }
    }
}