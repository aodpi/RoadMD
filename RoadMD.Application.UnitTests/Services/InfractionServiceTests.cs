using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RoadMD.Application.Dto.Infractions.Create;
using RoadMD.Application.Dto.Infractions.Update;
using RoadMD.Application.Services.Infractions;
using RoadMD.Application.UnitTests.Common.Mocks;
using Sieve.Models;

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
            var infractions = InfractionMock.GenerateRandomInfractions(Faker.Random.Number(2, 5));

            await Context.Infractions.AddRangeAsync(infractions);
            await Context.SaveChangesAsync();

            var randomInfraction = Faker.PickRandom(infractions);

            var result = await _infractionService.GetAsync(randomInfraction.Id);

            result.IsSuccess.Should().BeTrue();

            var infractionDto = result.Match(x => x, null);

            infractionDto.CategoryId.Should().Be(randomInfraction.CategoryId);
            infractionDto.Description.Should().Be(randomInfraction.Description);
            infractionDto.Name.Should().Be(randomInfraction.Name);
            infractionDto.Location!.Latitude.Should().Be(randomInfraction.Location.Latitude);
            infractionDto.Location.Longitude.Should().Be(randomInfraction.Location.Longitude);
        }

        [Fact]
        public async Task GetInfractionsListAsync()
        {
            var infractions = InfractionMock.GenerateRandomInfractions(Faker.Random.Number(2, 20));

            await Context.Infractions.AddRangeAsync(infractions);
            await Context.SaveChangesAsync();

            var sieveModel = new Faker<SieveModel>()
                .StrictMode(true)
                .RuleFor(x => x.Page, 1)
                .RuleFor(x => x.PageSize, faker => faker.Random.Number(10, 50))
                .Ignore(x => x.Filters)
                .Ignore(x => x.Sorts)
                .Generate();

            var result = await _infractionService.GetListAsync(sieveModel);

            result.Should().NotBeNull();
            result.Items.Should().HaveCountLessOrEqualTo(sieveModel.PageSize.GetValueOrDefault());
            result.TotalCount.Should().Be(infractions.Count);
            result.TotalPages.Should().Be(infractions.Count / sieveModel.PageSize + 1);
        }

        [Fact]
        public async Task CreateInfractionAsync()
        {
            var infractionCategories =
                InfractionCategoryMock.GenerateRandomInfractionCategories(Faker.Random.Number(5, 10));

            await Context.InfractionCategories.AddRangeAsync(infractionCategories);
            await Context.SaveChangesAsync();

            var createInfractionLocationDto = new Faker<CreateInfractionLocationDto>()
                .StrictMode(true)
                .RuleFor(x => x.Latitude, faker => faker.Random.Float())
                .RuleFor(x => x.Longitude, faker => faker.Random.Float())
                .Generate();

            var createInfractionVehicleDto = new Faker<CreateInfractionVehicleDto>()
                .StrictMode(true)
                .RuleFor(x => x.Number, faker => faker.Vehicle.Vin())
                .Generate();

            var createInfractionDto = new Faker<CreateInfractionDto>()
                .StrictMode(true)
                .RuleFor(x => x.Name, faker => faker.Random.Word())
                .RuleFor(x => x.Description, faker => faker.Lorem.Paragraph())
                .RuleFor(x => x.CategoryId, faker => faker.PickRandom(infractionCategories).Id)
                .RuleFor(x => x.Location, createInfractionLocationDto)
                .RuleFor(x => x.Vehicle, createInfractionVehicleDto)
                .Ignore(x => x.Photos)
                .Generate();

            var result = await _infractionService.CreateAsync(createInfractionDto);

            result.IsSuccess.Should().BeTrue();

            var resultDto = result.Match(x => x, null);

            var dbEntity = await Context.Infractions
                .Include(x => x.Location)
                .Include(x => x.Vehicle)
                .Where(x => x.Id.Equals(resultDto.Id))
                .SingleOrDefaultAsync();

            dbEntity.Should().NotBeNull();

            dbEntity!.Name.Should().Be(createInfractionDto.Name);
            dbEntity.Description.Should().Be(createInfractionDto.Description);
            dbEntity.CategoryId.Should().Be(createInfractionDto.CategoryId);

            dbEntity.Location.Latitude.Should().Be(createInfractionDto.Location.Latitude);
            dbEntity.Location.Longitude.Should().Be(createInfractionDto.Location.Longitude);

            dbEntity.Vehicle.Number.Should().Be(createInfractionDto.Vehicle.Number);
        }

        [Fact]
        public async Task UpdateInfractionAsync()
        {
            var infractionCategories =
                InfractionCategoryMock.GenerateRandomInfractionCategories(Faker.Random.Number(5, 10));

            var infractions = InfractionMock.GenerateRandomInfractions(Faker.Random.Number(3, 5));

            await Context.InfractionCategories.AddRangeAsync(infractionCategories);
            await Context.Infractions.AddRangeAsync(infractions);
            await Context.SaveChangesAsync();

            var randomInfractionToUpdate = Faker.PickRandom(infractions);

            var updateInfractionLocationDto = new Faker<UpdateInfractionLocationDto>()
                .StrictMode(true)
                .RuleFor(x => x.Latitude, faker => faker.Random.Float())
                .RuleFor(x => x.Longitude, faker => faker.Random.Float())
                .Generate();

            var updateInfractionVehicleDto = new Faker<UpdateInfractionVehicleDto>()
                .RuleFor(x => x.Number, faker => faker.Vehicle.Vin())
                .Generate();

            var updateInfractionDto = new Faker<UpdateInfractionDto>()
                .StrictMode(true)
                .RuleFor(x => x.Id, randomInfractionToUpdate.Id)
                .RuleFor(x => x.Name, faker => faker.Lorem.Word())
                .RuleFor(x => x.Description, faker => faker.Lorem.Paragraph())
                .RuleFor(x => x.CategoryId, faker => faker.PickRandom(infractionCategories).Id)
                .RuleFor(x => x.Location, updateInfractionLocationDto)
                .RuleFor(x => x.Vehicle, updateInfractionVehicleDto)
                .Generate();

            var result = await _infractionService.UpdateAsync(updateInfractionDto);

            result.IsSuccess.Should().BeTrue();

            var dbEntity = await Context.Infractions
                .Include(x => x.Location)
                .Include(x => x.Vehicle)
                .Where(x => x.Id.Equals(updateInfractionDto.Id))
                .SingleOrDefaultAsync();

            dbEntity.Should().NotBeNull();
            dbEntity!.Name.Should().Be(updateInfractionDto.Name);
            dbEntity.Description.Should().Be(updateInfractionDto.Description);
            dbEntity.CategoryId.Should().Be(updateInfractionDto.CategoryId);

            dbEntity.Location.Latitude.Should().Be(updateInfractionDto.Location.Latitude);
            dbEntity.Location.Longitude.Should().Be(updateInfractionDto.Location.Longitude);

            dbEntity.Vehicle.Number.Should().Be(updateInfractionDto.Vehicle.Number);
        }

        [Fact]
        public async Task DeleteInfractionAsync()
        {
            var infractions = InfractionMock.GenerateRandomInfractions(Faker.Random.Number(3, 5));

            await Context.Infractions.AddRangeAsync(infractions);
            await Context.SaveChangesAsync();

            var randomInfractionToDelete = Faker.PickRandom(infractions);

            var result = await _infractionService.DeleteAsync(randomInfractionToDelete.Id);

            result.IsSuccess.Should().BeTrue();

            var dbEntity =
                await Context.Infractions.SingleOrDefaultAsync(x => x.Id.Equals(randomInfractionToDelete.Id));

            dbEntity.Should().BeNull("Because entity was be deleted");
        }
    }
}