using Mapster;
using RoadMD.Domain.Entities;

namespace RoadMD.Application.Dto.Infractions
{
    public class InfractionPhotoDto : IRegister
    {
        public string? Name { get; init; }
        public string? Url { get; init; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Photo, InfractionPhotoDto>()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Url, src => src.Url);
        }
    }
}