using MapsterMapper;
using RoadMD.Application.UnitTests.Common.Factories;
using RoadMD.EntityFrameworkCore;
using Sieve.Services;

namespace RoadMD.Application.UnitTests
{
    [CollectionDefinition(nameof(QueryCollection))]
    public class QueryCollection : ICollectionFixture<TestFixture>
    {
    }

    public class TestFixture : IDisposable
    {
        public TestFixture()
        {
            Context = ApplicationDbContextFactory.Create();
            SieveProcessor = SieveProcessorFactory.Create();
            Mapper = MapperFactory.Create();
            Faker = FakerFactory.Create();
        }

        public ApplicationDbContext Context { get; }
        public ISieveProcessor SieveProcessor { get; }
        public IMapper Mapper { get; }
        public Faker Faker { get; }

        // Test Cleanup
        public void Dispose()
        {
            Context.Dispose();
        }
    }
}