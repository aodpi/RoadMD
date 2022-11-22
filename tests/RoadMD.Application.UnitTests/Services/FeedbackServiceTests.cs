using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RoadMD.Application.Dto.Feedbacks;
using RoadMD.Application.Services.Feedbacks;
using RoadMD.Application.UnitTests.Common.Mocks;
using Sieve.Models;

namespace RoadMD.Application.UnitTests.Services
{
    public class FeedbackServiceTests : TestFixture
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackServiceTests()
        {
            var feedbackServiceLoggerMock = new Mock<ILogger<FeedbackService>>();
            _feedbackService = new FeedbackService(Context, Mapper, SieveProcessor, feedbackServiceLoggerMock.Object);
        }

        [Fact]
        public async Task GetFeedbackAsync()
        {
            var feedbacks = FeedbackMock.GenerateRandomFeedbacks(Faker.Random.Number(2, 5));
            await Context.Feedbacks.AddRangeAsync(feedbacks);
            await Context.SaveChangesAsync();

            var randomFeedback = Faker.PickRandom(feedbacks);

            var result = await _feedbackService.GetAsync(randomFeedback.Id);

            var feedbackDto = result.Match(x => x, null);

            result.IsSuccess.Should().BeTrue();

            feedbackDto.Should().NotBeNull();
            feedbackDto.Id.Should().Be(randomFeedback.Id);
            feedbackDto.Subject.Should().Be(randomFeedback.Subject);
            feedbackDto.Description.Should().Be(randomFeedback.Description);
            feedbackDto.UserName.Should().Be(randomFeedback.UserName);
            feedbackDto.UserEmail.Should().Be(randomFeedback.UserEmail);
        }

        [Fact]
        public async Task GetFeedbacksListAsync()
        {
            var feedbacks = FeedbackMock.GenerateRandomFeedbacks(Faker.Random.Number(2, 30));
            await Context.Feedbacks.AddRangeAsync(feedbacks);
            await Context.SaveChangesAsync();

            var sieveModel = new Faker<SieveModel>()
                .StrictMode(true)
                .RuleFor(x => x.Page, 1)
                .RuleFor(x => x.PageSize, faker => faker.Random.Number(10, 50))
                .Ignore(x => x.Filters)
                .Ignore(x => x.Sorts)
                .Generate();

            var result = await _feedbackService.GetListAsync(sieveModel);

            result.Should().NotBeNull();
            result.Items.Should().HaveCountLessOrEqualTo(sieveModel.PageSize.GetValueOrDefault());
            result.TotalCount.Should().Be(feedbacks.Count);
            result.TotalPages.Should().Be(feedbacks.Count / sieveModel.PageSize + 1);
        }

        [Fact]
        public async Task CreateFeedbackAsync()
        {
            var createFeedbackDto = new Faker<CreateFeedbackDto>()
                .StrictMode(true)
                .RuleFor(x => x.Subject, faker => faker.Lorem.Slug(5))
                .RuleFor(x => x.Description, faker => faker.Lorem.Paragraph())
                .RuleFor(x => x.UserName, faker => faker.Person.FullName)
                .RuleFor(x => x.UserEmail, faker => faker.Person.Email)
                .Generate();


            var result = await _feedbackService.CreateAsync(createFeedbackDto);

            result.IsSuccess.Should().BeTrue();

            var feedbackDto = result.Match(x => x, null);

            var dbEntity = await Context.Feedbacks.SingleOrDefaultAsync(x => x.Id.Equals(feedbackDto.Id));

            dbEntity.Should().NotBeNull();
            dbEntity!.Subject.Should().Be(createFeedbackDto.Subject);
            dbEntity.Description.Should().Be(createFeedbackDto.Description);
            dbEntity.UserName.Should().Be(createFeedbackDto.UserName);
            dbEntity.UserEmail.Should().Be(createFeedbackDto.UserEmail);
        }

        [Fact]
        public async Task UpdateFeedbackAsync()
        {
            var feedbacks = FeedbackMock.GenerateRandomFeedbacks(Faker.Random.Number(2, 5));
            await Context.Feedbacks.AddRangeAsync(feedbacks);
            await Context.SaveChangesAsync();

            var randomFeedbackToUpdate = Faker.PickRandom(feedbacks);

            var updateFeedbackDto = new Faker<UpdateFeedbackDto>()
                .StrictMode(true)
                .RuleFor(x => x.Id, randomFeedbackToUpdate.Id)
                .RuleFor(x => x.Subject, faker => faker.Lorem.Slug(5))
                .RuleFor(x => x.Description, faker => faker.Lorem.Paragraph())
                .RuleFor(x => x.UserName, faker => faker.Person.UserName)
                .RuleFor(x => x.UserEmail, faker => faker.Person.Email)
                .Generate();

            var result = await _feedbackService.UpdateAsync(updateFeedbackDto);

            result.IsSuccess.Should().BeTrue();

            var dbEntity = await Context.Feedbacks.SingleOrDefaultAsync(x => x.Id.Equals(updateFeedbackDto.Id));

            dbEntity.Should().NotBeNull();
            dbEntity!.Subject.Should().Be(updateFeedbackDto.Subject);
            dbEntity.Description.Should().Be(updateFeedbackDto.Description);
            dbEntity.UserName.Should().Be(updateFeedbackDto.UserName);
            dbEntity.UserEmail.Should().Be(updateFeedbackDto.UserEmail);
        }

        [Fact]
        public async Task DeleteFeedbackAsync()
        {
            var feedbacks = FeedbackMock.GenerateRandomFeedbacks(Faker.Random.Number(2, 5));
            await Context.Feedbacks.AddRangeAsync(feedbacks);
            await Context.SaveChangesAsync();

            var randomFeedbackToDelete = Faker.PickRandom(feedbacks);

            var result = await _feedbackService.DeleteAsync(randomFeedbackToDelete.Id);

            result.IsSuccess.Should().BeTrue();

            var dbEntity = await Context.Feedbacks.SingleOrDefaultAsync(x => x.Id.Equals(randomFeedbackToDelete.Id));

            dbEntity.Should().BeNull();
        }
    }
}