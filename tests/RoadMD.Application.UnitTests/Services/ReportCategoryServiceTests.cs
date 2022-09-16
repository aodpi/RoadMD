using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RoadMD.Application.Dto.ReportCategories;
using RoadMD.Application.Services.ReportCategories;
using RoadMD.Application.UnitTests.Common.Mocks;

namespace RoadMD.Application.UnitTests.Services
{
    public class ReportCategoryServiceTests : TestFixture
    {
        private readonly IReportCategoryService _reportCategoryService;

        public ReportCategoryServiceTests()
        {
            var loggerMock = new Mock<ILogger<ReportCategoryService>>();
            _reportCategoryService = new ReportCategoryService(Context, Mapper, loggerMock.Object, SieveProcessor);
        }

        [Fact]
        public async Task GetReportCategoryAsync()
        {
            var reportCategories = ReportCategoryMock.GenerateRandomReportCategories(Faker.Random.Number(4, 10));

            await Context.ReportCategories.AddRangeAsync(reportCategories);
            await Context.SaveChangesAsync();

            var randomReportCategory = Faker.PickRandom(reportCategories);

            var result = await _reportCategoryService.GetAsync(randomReportCategory.Id);

            result.IsSuccess.Should().BeTrue();

            var resultDto = result.Match(x => x, null);

            resultDto.Id.Should().Be(randomReportCategory.Id);
            resultDto.Name.Should().Be(randomReportCategory.Name);
        }

        [Fact]
        public async Task GetReportCategoriesSelectListAsync()
        {
            var reportCategories = ReportCategoryMock.GenerateRandomReportCategories(Faker.Random.Number(4, 15));

            await Context.ReportCategories.AddRangeAsync(reportCategories);
            await Context.SaveChangesAsync();

            var items = await _reportCategoryService.GetSelectListAsync();

            items.Should().HaveCount(reportCategories.Count);
        }

        [Fact]
        public async Task CreateReportCategoryAsync()
        {
            var createReportCategoryDto = new Faker<CreateReportCategoryDto>()
                .StrictMode(true)
                .RuleFor(x => x.Name, faker => faker.Lorem.Word())
                .Generate();

            var result = await _reportCategoryService.CreateAsync(createReportCategoryDto);

            result.IsSuccess.Should().BeTrue();

            var resultDto = result.Match(x => x, null);

            var dbEntity = await Context.ReportCategories
                .Where(x => x.Id.Equals(resultDto.Id))
                .SingleOrDefaultAsync();

            dbEntity.Should().NotBeNull();

            dbEntity!.Name.Should().Be(createReportCategoryDto.Name);
        }

        [Fact]
        public async Task UpdateReportCategoryAsync()
        {
            var reportCategories = ReportCategoryMock.GenerateRandomReportCategories(Faker.Random.Number(2, 4));

            await Context.ReportCategories.AddRangeAsync(reportCategories);
            await Context.SaveChangesAsync();

            var randomReportCategoryToUpdate = Faker.PickRandom(reportCategories);

            var updateReportCategoryDto = new Faker<UpdateReportCategoryDto>()
                .StrictMode(true)
                .RuleFor(x => x.Id, randomReportCategoryToUpdate.Id)
                .RuleFor(x => x.Name, faker => faker.Lorem.Word())
                .Generate();

            var result = await _reportCategoryService.UpdateAsync(updateReportCategoryDto);

            result.IsSuccess.Should().BeTrue();

            var dbEntity = await Context.ReportCategories
                .Where(x => x.Id.Equals(updateReportCategoryDto.Id))
                .SingleOrDefaultAsync();

            dbEntity.Should().NotBeNull();

            dbEntity!.Name.Should().Be(updateReportCategoryDto.Name);
        }

        [Fact]
        public async Task DeleteReportCategoryAsync()
        {
            var reportCategories = ReportCategoryMock.GenerateRandomReportCategories(Faker.Random.Number(2, 4));

            await Context.ReportCategories.AddRangeAsync(reportCategories);
            await Context.SaveChangesAsync();

            var randomReportCategoryToDelete = Faker.PickRandom(reportCategories);

            var result = await _reportCategoryService.DeleteAsync(randomReportCategoryToDelete.Id);

            result.IsSuccess.Should().BeTrue();

            var dbEntity = await Context.ReportCategories
                .Where(x => x.Id.Equals(randomReportCategoryToDelete.Id))
                .SingleOrDefaultAsync();

            dbEntity.Should().BeNull();
        }
    }
}