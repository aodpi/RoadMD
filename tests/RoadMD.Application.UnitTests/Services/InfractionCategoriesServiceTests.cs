using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RoadMD.Application.Dto.Common;
using RoadMD.Application.Dto.InfractionCategories;
using RoadMD.Application.Services.InfractionCategories;
using RoadMD.Application.UnitTests.Common.Mocks;

namespace RoadMD.Application.UnitTests.Services
{
    public class InfractionCategoriesServiceTests : TestFixture
    {
        private readonly IInfractionCategoriesService _infractionCategoriesService;

        public InfractionCategoriesServiceTests()
        {
            var mock = new Mock<ILogger<InfractionCategoriesService>>();
            _infractionCategoriesService =
                new InfractionCategoriesService(Context, Mapper, mock.Object, SieveProcessor);
        }

        [Fact]
        public async Task GetInfractionCategoryAsync()
        {
            var infractionCategories =
                InfractionCategoryMock.GenerateRandomInfractionCategories(Faker.Random.Number(2, 10));

            await Context.InfractionCategories.AddRangeAsync(infractionCategories);
            await Context.SaveChangesAsync();

            var randomInfractionCategory = new Faker().PickRandom(infractionCategories);

            var result = await _infractionCategoriesService.GetAsync(randomInfractionCategory.Id);

            result.IsSuccess.Should().BeTrue();

            var dto = result.Match(obj => obj, null);

            dto.Id.Should().Be(randomInfractionCategory.Id);
            dto.Name.Should().Be(randomInfractionCategory.Name);
        }

        [Fact]
        public async Task GetInfractionCategoriesSelectListAsync()
        {
            var infractionCategories =
                InfractionCategoryMock.GenerateRandomInfractionCategories(Faker.Random.Number(2, 10));

            await Context.InfractionCategories.AddRangeAsync(infractionCategories);
            await Context.SaveChangesAsync();

            var result = await _infractionCategoriesService.GetSelectListAsync();

            result.Should().NotBeNull();

            result.Should().AllBeAssignableTo<LookupDto>();
        }

        [Fact]
        public async Task CreateInfractionCategoryAsync()
        {
            var createInfractionCategoryDto = new Faker<CreateInfractionCategoryDto>()
                .StrictMode(true)
                .RuleFor(x => x.Name, faker => faker.Random.Word())
                .Generate();

            var result = await _infractionCategoriesService.CreateAsync(createInfractionCategoryDto);

            result.IsSuccess.Should().BeTrue();
            var resultDto = result.Match(obj => obj, null);

            var infractionCategory = await Context.InfractionCategories
                .SingleOrDefaultAsync(x => x.Id.Equals(resultDto.Id));

            infractionCategory.Should().NotBeNull();
            infractionCategory!.Name.Should().Be(resultDto.Name);
        }

        [Fact]
        public async Task UpdateInfractionCategoryAsync()
        {
            var infractionCategories =
                InfractionCategoryMock.GenerateRandomInfractionCategories(Faker.Random.Number(2, 10));

            await Context.InfractionCategories.AddRangeAsync(infractionCategories);
            await Context.SaveChangesAsync();

            var randomInfractionCategory = new Faker().PickRandom(infractionCategories);

            var updateInfractionCategoryDto = new Faker<UpdateInfractionCategoryDto>()
                .StrictMode(true)
                .RuleFor(x => x.Id, randomInfractionCategory.Id)
                .RuleFor(x => x.Name, faker => faker.Random.Word())
                .Generate();

            var result = await _infractionCategoriesService.UpdateAsync(updateInfractionCategoryDto);

            var infractionCategory = await Context.InfractionCategories
                .SingleOrDefaultAsync(x => x.Id.Equals(updateInfractionCategoryDto.Id));

            result.IsSuccess.Should().BeTrue();
            infractionCategory.Should().NotBeNull();
            infractionCategory!.Name.Should().Be(updateInfractionCategoryDto.Name);
        }

        [Fact]
        public async Task DeleteInfractionCategoryAsync()
        {
            var infractionCategories =
                InfractionCategoryMock.GenerateRandomInfractionCategories(Faker.Random.Number(2, 10));

            await Context.InfractionCategories.AddRangeAsync(infractionCategories);
            await Context.SaveChangesAsync();

            var randomInfractionCategory = new Faker().PickRandom(infractionCategories);

            var result = await _infractionCategoriesService.DeleteAsync(randomInfractionCategory.Id);

            result.IsSuccess.Should().BeTrue();

            var infractionCategory = await Context.InfractionCategories
                .SingleOrDefaultAsync(x => x.Id.Equals(randomInfractionCategory.Id));

            infractionCategory.Should().BeNull();
        }

        [Fact]
        public async Task BulkDeleteInfractionCategoriesAsync()
        {
            var infractionCategories =
                InfractionCategoryMock.GenerateRandomInfractionCategories(Faker.Random.Number(2, 50));

            await Context.InfractionCategories.AddRangeAsync(infractionCategories);
            await Context.SaveChangesAsync();

            var randomInfractionCategoriesIds = new Faker()
                .PickRandom(infractionCategories, Faker.Random.Number(0, infractionCategories.Count))
                .Select(x => x.Id)
                .ToArray();

            var result = await _infractionCategoriesService.BulkDeleteAsync(randomInfractionCategoriesIds, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            var infractionCategoriesCount = await Context.InfractionCategories
                .Where(x => randomInfractionCategoriesIds.Contains(x.Id))
                .CountAsync();

            infractionCategoriesCount.Should().Be(0);
        }
    }
}