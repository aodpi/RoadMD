using RoadMD.Domain.Entities;

namespace RoadMD.Application.UnitTests.Common.Mocks
{
    public static class InfractionCategoryMock
    {
        public static Faker<InfractionCategory> GetInfractionCategoryFaker()
        {
            var fakerInfractionCategory = new Faker<InfractionCategory>()
                .StrictMode(true)
                .RuleFor(x => x.Id, faker => faker.Random.Guid())
                .RuleFor(x => x.Name, faker => faker.Lorem.Sentence(4));

            return fakerInfractionCategory;
        }

        public static List<InfractionCategory> GenerateRandomInfractionCategories(int count)
        {
            return GetInfractionCategoryFaker().Generate(count);
        }

        public static InfractionCategory GenerateRandomInfractionCategory()
        {
            return GetInfractionCategoryFaker().Generate();
        }
    }
}