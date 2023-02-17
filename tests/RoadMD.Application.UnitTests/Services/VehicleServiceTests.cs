using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoadMD.Application.Dto.Vehicles;
using RoadMD.Application.Services.Vehicles;
using RoadMD.Application.UnitTests.Common.Mocks;
using Sieve.Models;

namespace RoadMD.Application.UnitTests.Services
{
    public class VehicleServiceTests : TestFixture
    {
        private readonly IVehicleService _vehicleService;

        public VehicleServiceTests()
        {
            var loggerMock = new Mock<ILogger<VehicleService>>();
            _vehicleService = new VehicleService(Context, Mapper, loggerMock.Object, SieveProcessor);
        }

        [Fact]
        public async Task GetVehicleAsync()
        {
            var vehicles = VehicleMock.GenerateRandomVehicles(Faker.Random.Number(5, 10));

            await Context.Vehicles.AddRangeAsync(vehicles);
            await Context.SaveChangesAsync();

            var randomVehicle = Faker.PickRandom(vehicles);

            var result = await _vehicleService.GetAsync(randomVehicle.Id);

            result.IsSuccess.Should().BeTrue();

            var resultDto = result.Match(x => x, null);

            resultDto.Id.Should().Be(randomVehicle.Id);
            resultDto.Number.Should().Be(randomVehicle.Number);
        }

        [Fact]
        public async Task GetVehiclesListAsync()
        {
            var vehicles = VehicleMock.GenerateRandomVehicles(Faker.Random.Number(5, 20));

            await Context.Vehicles.AddRangeAsync(vehicles);
            await Context.SaveChangesAsync();


            var sieveModel = new Faker<SieveModel>()
                .StrictMode(true)
                .RuleFor(x => x.Page, 1)
                .RuleFor(x => x.PageSize, faker => faker.Random.Number(10, 50))
                .Ignore(x => x.Filters)
                .Ignore(x => x.Sorts)
                .Generate();

            var result = await _vehicleService.GetListAsync(sieveModel);

            result.Should().NotBeNull();
            result.Items.Should().HaveCountLessOrEqualTo(sieveModel.PageSize.GetValueOrDefault());
            result.TotalCount.Should().Be(vehicles.Count);
            result.TotalPages.Should().Be(vehicles.Count / sieveModel.PageSize + 1);
        }

        [Fact]
        public async Task CreateVehicleAsync()
        {
            var createVehicleDto = new Faker<CreateVehicleDto>()
                .StrictMode(true)
                .RuleFor(x => x.Number, faker => faker.Vehicle.Vin())
                .Generate();

            var result = await _vehicleService.CreateAsync(createVehicleDto);

            result.IsSuccess.Should().BeTrue();

            var resultDto = result.Match(x => x, null);

            var dbEntity = await Context.Vehicles
                .Where(x => x.Id.Equals(resultDto.Id))
                .SingleOrDefaultAsync();

            dbEntity.Should().NotBeNull();
            dbEntity!.Number.Should().Be(createVehicleDto.Number);
        }

        [Fact]
        public async Task UpdateVehicleAsync()
        {
            var vehicles = VehicleMock.GenerateRandomVehicles(Faker.Random.Number(5, 10));

            await Context.Vehicles.AddRangeAsync(vehicles);
            await Context.SaveChangesAsync();

            var randomVehicleToUpdate = Faker.PickRandom(vehicles);

            var updateVehicleDto = new Faker<UpdateVehicleDto>()
                .StrictMode(true)
                .RuleFor(x => x.Id, randomVehicleToUpdate.Id)
                .RuleFor(x => x.Number, faker => faker.Vehicle.Vin())
                .Generate();

            var result = await _vehicleService.UpdateAsync(updateVehicleDto);

            result.IsSuccess.Should().BeTrue();

            var dbEntity = await Context.Vehicles
                .Where(x => x.Id.Equals(updateVehicleDto.Id))
                .SingleOrDefaultAsync();

            dbEntity.Should().NotBeNull();
            dbEntity!.Number.Should().Be(updateVehicleDto.Number);
        }

        [Fact]
        public async Task DeleteVehicleAsync()
        {
            var vehicles = VehicleMock.GenerateRandomVehicles(Faker.Random.Number(2, 5));

            await Context.Vehicles.AddRangeAsync(vehicles);
            await Context.SaveChangesAsync();

            var randomVehicleToDelete = Faker.PickRandom(vehicles);

            var result = await _vehicleService.DeleteAsync(randomVehicleToDelete.Id);

            result.IsSuccess.Should().BeTrue();


            var dbEntity = await Context.Vehicles
                .Where(x => x.Id.Equals(randomVehicleToDelete.Id))
                .SingleOrDefaultAsync();

            dbEntity.Should().BeNull();
        }
    }
}