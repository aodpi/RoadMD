using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoadMD.Application.Dto.InfractionReports;
using RoadMD.Application.Services.InfractionReports;
using RoadMD.Application.UnitTests.Common.Mocks;

namespace RoadMD.Application.UnitTests.Services
{
    public class InfractionReportServiceTests : TestFixture
    {
        private readonly IInfractionReportService _infractionReportService;

        public InfractionReportServiceTests()
        {
            var loggerMock = new Mock<ILogger<InfractionReportService>>();
            _infractionReportService = new InfractionReportService(Context, Mapper,
                loggerMock.Object, SieveProcessor);
        }

        [Fact]
        public async Task GetInfractionReportAsync()
        {
            var infractionReports = InfractionReportMock.GenerateRandomInfractionReports(Faker.Random.Number(5, 10));
            await Context.InfractionReports.AddRangeAsync(infractionReports);
            await Context.SaveChangesAsync();


            var randomInfractionReport = Faker.PickRandom(infractionReports);

            var result = await _infractionReportService.GetAsync(randomInfractionReport.Id);

            result.IsSuccess.Should().BeTrue();

            var resultDto = result.Match(x => x, null);

            resultDto.Should().NotBeNull();

            resultDto.Id.Should().Be(randomInfractionReport.Id);
            resultDto.Description.Should().Be(randomInfractionReport.Description);
            resultDto.ReportCategoryId.Should().Be(randomInfractionReport.ReportCategoryId);
            resultDto.InfractionId.Should().Be(randomInfractionReport.InfractionId);
        }

        [Fact]
        public async Task CreateInfractionReportAsync()
        {
            var createInfractionReportDto = new Faker<CreateInfractionReportDto>()
                .StrictMode(true)
                .RuleFor(x => x.InfractionId, faker => faker.Random.Guid())
                .RuleFor(x => x.ReportCategoryId, faker => faker.Random.Guid())
                .RuleFor(x => x.Description, faker => faker.Lorem.Paragraph())
                .Generate();

            var result = await _infractionReportService.CreateAsync(createInfractionReportDto);

            result.IsSuccess.Should().BeTrue();
            var entityId = result.Match(x => x.Id, null);

            var dbEntity = await Context.InfractionReports.SingleOrDefaultAsync(x => x.Id.Equals(entityId));
            dbEntity.Should().NotBeNull();

            dbEntity!.InfractionId.Should().Be(createInfractionReportDto.InfractionId);
            dbEntity.ReportCategoryId.Should().Be(createInfractionReportDto.ReportCategoryId);
            dbEntity.Description.Should().Be(createInfractionReportDto.Description);
        }

        [Fact]
        public async Task UpdateInfractionReportAsync()
        {
            var infractionReports = InfractionReportMock.GenerateRandomInfractionReports(Faker.Random.Number(5, 10));
            await Context.InfractionReports.AddRangeAsync(infractionReports);
            await Context.SaveChangesAsync();


            var randomInfractionReport = Faker.PickRandom(infractionReports);

            var updateInfractionReportDto = new Faker<UpdateInfractionReportDto>()
                .StrictMode(true)
                .RuleFor(x => x.Id, randomInfractionReport.Id)
                .RuleFor(x => x.InfractionId, faker => faker.Random.Guid())
                .RuleFor(x => x.ReportCategoryId, faker => faker.Random.Guid())
                .RuleFor(x => x.Description, faker => faker.Lorem.Paragraph())
                .Generate();

            var result = await _infractionReportService.UpdateAsync(updateInfractionReportDto);

            result.IsSuccess.Should().BeTrue();

            var dbEntity = await Context.InfractionReports
                .SingleOrDefaultAsync(x => x.Id.Equals(updateInfractionReportDto.Id));

            dbEntity.Should().NotBeNull();
            dbEntity!.InfractionId.Should().Be(updateInfractionReportDto.InfractionId);
            dbEntity.ReportCategoryId.Should().Be(updateInfractionReportDto.ReportCategoryId);
            dbEntity.Description.Should().Be(updateInfractionReportDto.Description);
        }

        [Fact]
        public async Task DeleteInfractionReportAsync()
        {
            var infractionReports = InfractionReportMock.GenerateRandomInfractionReports(Faker.Random.Number(5, 10));
            await Context.InfractionReports.AddRangeAsync(infractionReports);
            await Context.SaveChangesAsync();


            var randomInfractionReport = Faker.PickRandom(infractionReports);

            var result = await _infractionReportService.DeleteAsync(randomInfractionReport.Id);

            result.IsSuccess.Should().BeTrue();

            var dbEntity =
                await Context.InfractionReports.SingleOrDefaultAsync(x => x.Id.Equals(randomInfractionReport.Id));

            dbEntity.Should().BeNull();
        }

        [Fact]
        public async Task DeleteInfractionReportByInfraction()
        {
            var infraction = InfractionMock.GenerateRandomInfraction();
            var infractionReports = InfractionReportMock.GenerateRandomInfractionReports(Faker.Random.Number(5, 10));
            foreach (var report in infractionReports)
            {
                report.InfractionId = infraction.Id;
            }

            await Context.InfractionReports.AddRangeAsync(infractionReports);
            await Context.SaveChangesAsync();

            var randomInfractionReport = Faker.PickRandom(infractionReports);

            var result = await _infractionReportService.DeleteAsync(randomInfractionReport.Id, infraction.Id);

            result.IsSuccess.Should().BeTrue();


            var dbEntity =
                await Context.InfractionReports.SingleOrDefaultAsync(x =>
                    x.Id.Equals(randomInfractionReport.Id) && x.InfractionId.Equals(infraction.Id));

            dbEntity.Should().BeNull();
        }
    }
}