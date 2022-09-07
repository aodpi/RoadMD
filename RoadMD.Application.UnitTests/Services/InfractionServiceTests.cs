using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RoadMD.Application.Dto.Infractions.Create;
using RoadMD.Application.Services.Infractions;
using RoadMD.Application.UnitTests.Common.Mocks;
using RoadMD.Domain.Entities;

namespace RoadMD.Application.UnitTests.Services
{
    public class InfractionServiceTests : TestFixture
    {
        private readonly IInfractionService _infractionService;

        public InfractionServiceTests()
        {
            var loggerMock = new Mock<ILogger<InfractionService>>();
            var photoStorageMok = new PhotoStorageServiceMock()
                .MockStorePhoto(Faker.Random.Word(), It.IsAny<Stream>());

            _infractionService = new InfractionService(Context, Mapper, loggerMock.Object, photoStorageMok.Object,
                SieveProcessor);
        }

        [Fact]
        public async Task GetInfractionAsync()
        {
            var vehicleFaker = new Faker<Vehicle>()
                .StrictMode(true)
                .RuleFor(x => x.Id, faker => faker.Random.Guid())
                .RuleFor(x => x.Number, faker => faker.Vehicle.Vin())
                .Ignore(x => x.Infractions);

            var infractions = InfractionMock.GetInfractionFaker()
                .RuleFor(x => x.Vehicle, vehicleFaker.Generate())
                .Generate(Faker.Random.Number(2, 5));

            await Context.Infractions.AddRangeAsync(infractions);
            await Context.SaveChangesAsync();

            var randomInfraction = Faker.PickRandom(infractions);

            var result = await _infractionService.GetAsync(randomInfraction.Id);

            result.IsSuccess.Should().BeTrue();

            var infractionDto = result.Match(x => x, null);

            infractionDto.CategoryId.Should().Be(randomInfraction.CategoryId);
            infractionDto.Description.Should().Be(randomInfraction.Description);
            infractionDto.Name.Should().Be(randomInfraction.Name);
            infractionDto.Location.Should().Be(randomInfraction.LocationId);
        }
    }
}