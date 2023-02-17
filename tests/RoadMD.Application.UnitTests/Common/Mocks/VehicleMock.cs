using RoadMD.Domain.Entities;

namespace RoadMD.Application.UnitTests.Common.Mocks
{
    internal static class VehicleMock
    {
        internal static Faker<Vehicle> GetVehicleFaker()
        {
            var vehicleFaker = new Faker<Vehicle>()
                .StrictMode(true)
                .RuleFor(x => x.Id, faker => faker.Random.Guid())
                .RuleFor(x => x.Number, faker => faker.Vehicle.Vin())
                .Ignore(x => x.Infractions);

            return vehicleFaker;
        }

        internal static List<Vehicle> GenerateRandomVehicles(int count)
        {
            return GetVehicleFaker().Generate(count);
        }

        internal static Vehicle GenerateRandomVehicle()
        {
            return GetVehicleFaker().Generate();
        }
    }
}