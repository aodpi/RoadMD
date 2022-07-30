using Mapster;
using RoadMD.Domain.Entities;

namespace RoadMD.Application.Dto.InfractionCategories
{
    public class InfractionCategoryDto : IRegister
    {
        public Guid Id { get; init; }
        public string Name { get; init; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<InfractionCategory, InfractionCategoryDto>();
        }
    }
}