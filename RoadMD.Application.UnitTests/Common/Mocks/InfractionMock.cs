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
                .RuleFor(x => x.LocationId, faker => faker.Random.Guid())
                .RuleFor(x => x.VehicleId, faker => faker.Random.Guid())
                .Ignore(x => x.Location)
                .Ignore(x => x.Vehicle)
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