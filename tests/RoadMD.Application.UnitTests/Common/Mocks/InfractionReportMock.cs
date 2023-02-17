using RoadMD.Domain.Entities;

namespace RoadMD.Application.UnitTests.Common.Mocks
{
    internal static class InfractionReportMock
    {
        internal static Faker<InfractionReport> GetInfractionReportFaker(Guid? infractionId = default, Guid? categoryId = default)
        {
            var infractionFaker = InfractionMock.GetInfractionFaker();

            var reportCategoryFaker = ReportCategoryMock.GetReportCategoryFaker();
            var infractionReport = new Faker<InfractionReport>()
                .StrictMode(true)
                .RuleFor(x => x.Id, faker => faker.Random.Guid())
                .RuleFor(x => x.Description, faker => faker.Lorem.Paragraph())
                .RuleFor(x => x.Infraction, infractionFaker.Generate())
                .RuleFor(x => x.ReportCategory, reportCategoryFaker.Generate())
                .Ignore(x => x.InfractionId)
                .Ignore(x => x.ReportCategoryId);

            return infractionReport;
        }

        internal static List<InfractionReport> GenerateRandomInfractionReports(int count)
        {
            return GetInfractionReportFaker().Generate(count);
        }

        internal static InfractionReport GenerateRandomInfractionReport()
        {
            return GetInfractionReportFaker().Generate();
        }
    }
}