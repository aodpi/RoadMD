using RoadMD.Domain.Entities;

namespace RoadMD.Application.UnitTests.Common.Mocks
{
    public static class InfractionReportMock
    {
        public static Faker<InfractionReport> GetInfractionReportFaker()
        {
            var infractionReport = new Faker<InfractionReport>()
                .StrictMode(true)
                .RuleFor(x => x.Id, faker => faker.Random.Guid())
                .RuleFor(x => x.Description, faker => faker.Lorem.Paragraph())
                .Ignore(x => x.Infraction)
                .Ignore(x => x.ReportCategory)
                .Ignore(x => x.InfractionId)
                .Ignore(x => x.ReportCategoryId);

            return infractionReport;
        }

        public static List<InfractionReport> GenerateRandomInfractionReports(int count)
        {
            return GetInfractionReportFaker().Generate(count);
        }

        public static InfractionReport GenerateRandomInfractionReport()
        {
            return GetInfractionReportFaker().Generate();
        }
    }
}