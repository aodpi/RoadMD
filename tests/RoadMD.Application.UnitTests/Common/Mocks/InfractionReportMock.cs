using RoadMD.Domain.Entities;

namespace RoadMD.Application.UnitTests.Common.Mocks
{
    public static class InfractionReportMock
    {
        public static Faker<InfractionReport> GetInfractionReportFaker(Guid? infractionId = default, Guid? categoryId = default)
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