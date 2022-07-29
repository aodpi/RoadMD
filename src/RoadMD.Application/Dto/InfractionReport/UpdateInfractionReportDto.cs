namespace RoadMD.Application.Dto.InfractionReport
{
    public class UpdateInfractionReportDto : CreateInfractionReportDto
    {
        public Guid Id { get; init; }
    }
}