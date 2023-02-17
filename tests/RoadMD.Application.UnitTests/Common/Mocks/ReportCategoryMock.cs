using RoadMD.Domain.Entities;

namespace RoadMD.Application.UnitTests.Common.Mocks
{
    internal static class ReportCategoryMock
    {
        internal static Faker<ReportCategory> GetReportCategoryFaker()
        {
            var reportCategoryFaker = new Faker<ReportCategory>()
                .StrictMode(true)
                .RuleFor(x => x.Id, faker => faker.Random.Guid())
                .RuleFor(x => x.Name, faker => faker.Lorem.Word())
                .Ignore(x => x.InfractionReports);

            return reportCategoryFaker;
        }

        internal static List<ReportCategory> GenerateRandomReportCategories(int count)
        {
            return GetReportCategoryFaker().Generate(count);
        }

        internal static ReportCategory GenerateRandomReportCategory()
        {
            return GetReportCategoryFaker().Generate();
        }
    }
}