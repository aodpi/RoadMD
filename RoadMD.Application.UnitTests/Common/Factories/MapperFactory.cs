using System.Reflection;
using Mapster;
using MapsterMapper;

namespace RoadMD.Application.UnitTests.Common.Factories
{
    public static class MapperFactory
    {
        public static IMapper Create()
        {
            var config = new TypeAdapterConfig();

            config.Scan(Assembly.GetExecutingAssembly());

            return new Mapper(config);
        }
    }
}