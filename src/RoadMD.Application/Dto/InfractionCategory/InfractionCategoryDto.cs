using Mapster;

namespace RoadMD.Application.Dto.InfractionCategory
{
    public class InfractionCategoryDto : IRegister
    {
        public Guid Id { get; init; }
        public string Name { get; init; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Domain.Entities.InfractionCategory, InfractionCategoryDto>();
        }
    }
}