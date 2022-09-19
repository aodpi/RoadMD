using RoadMD.Domain.Entities;

namespace RoadMD.Application.UnitTests.Common.Mocks
{
    public static class FeedbackMock
    {
        public static Faker<Feedback> GetFeedbackFaker()
        {
            var fakerFeedback = new Faker<Feedback>()
                .StrictMode(true)
                .RuleFor(x => x.Id, faker => faker.Random.Guid())
                .RuleFor(x => x.Subject, faker => faker.Lorem.Slug(8))
                .RuleFor(x => x.Description, faker => faker.Lorem.Paragraph())
                .RuleFor(x => x.UserEmail, faker => faker.Person.Email)
                .RuleFor(x => x.UserName, faker => faker.Person.UserName);

            return fakerFeedback;
        }

        public static List<Feedback> GenerateRandomFeedbacks(int count)
        {
            return GetFeedbackFaker().Generate(count);
        }

        public static Feedback GenerateRandomFeedback()
        {
            return GetFeedbackFaker().Generate();
        }
    }
}