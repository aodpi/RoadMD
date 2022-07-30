using Mapster;
using RoadMD.Domain.Entities;

namespace RoadMD.Application.Dto.ReportCategories
{
    public class ReportCategoryDto : IRegister
    {
        public Guid Id { get; init; }
        public string Name { get; init; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ReportCategory, ReportCategoryDto>();
        }
    }
}