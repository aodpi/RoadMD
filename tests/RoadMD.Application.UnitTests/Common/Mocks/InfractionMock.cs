using RoadMD.Domain.Entities;

namespace RoadMD.Application.UnitTests.Common.Mocks
{
    public static class InfractionMock
    {
        public static Faker<Infraction> GetInfractionFaker()
        {
            var fakerInfraction = new Faker<Infraction>()
                .StrictMode(true)
                .RuleFor(x => x.Id, faker => faker.Random.Guid())
                .RuleFor(x => x.Name, faker => faker.Lorem.Word())
                .RuleFor(x => x.Description, faker => faker.Lorem.Paragraph())
                .RuleFor(x => x.CategoryId, faker => faker.Random.Guid())
                .RuleFor(x => x.Location, (_, infraction) => new Faker<Location>()
                    .StrictMode(true)
                    .RuleFor(x => x.Id, x => x.Random.Guid())
                    .RuleFor(x => x.Latitude, x => x.Random.Float(0, 99f))
                    .RuleFor(x => x.Longitude, x => x.Random.Float(0, 99f))
                    .RuleFor(x => x.InfractionId, infraction.Id)
                    .Ignore(x => x.Infraction)
                    .Ignore(x => x.InfractionId)
                    .Generate()
                )
                .RuleFor(x => x.Vehicle, new Faker<Vehicle>()
                    .StrictMode(true)
                    .RuleFor(x => x.Id, vehicleFaker => vehicleFaker.Random.Guid())
                    .RuleFor(x => x.Number, vehicleFaker => vehicleFaker.Vehicle.Vin())
                    .Ignore(x => x.Infractions)
                    .Generate()
                )
                .Ignore(x => x.LocationId)
                .Ignore(x => x.VehicleId)
                .Ignore(x => x.Category)
                .Ignore(x => x.Photos)
                .Ignore(x => x.Reports);

            return fakerInfraction;
        }


        public static List<Infraction> GenerateRandomInfractions(int count)
        {
            return GetInfractionFaker().Generate(count);
        }

        public static Infraction GenerateRandomInfraction()
        {
            return GetInfractionFaker().Generate();
        }
    }
}