using Mapster;

namespace RoadMD.Application.Dto.ReportCategory
{
    public class ReportCategoryDto : IRegister
    {
        public Guid Id { get; init; }
        public string Name { get; init; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.ReportCategory, ReportCategoryDto>();
        }
    }
}