namespace RoadMD.Application.Dto.InfractionReports
{
    public class UpdateInfractionReportDto : CreateInfractionReportDto
    {
        public Guid Id { get; init; }
    }
}