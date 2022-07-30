using Mapster;
using RoadMD.Domain.Entities;

namespace RoadMD.Application.Dto.InfractionReports
{
    public class InfractionReportDto : IRegister
    {
        public Guid Id { get; init; }
        public Guid InfractionId { get; init; }
        public Guid ReportCategoryId { get; init; }
        public string Description { get; init; }

        /// <inheritdoc />
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<InfractionReport, InfractionReportDto>();
        }
    }
}