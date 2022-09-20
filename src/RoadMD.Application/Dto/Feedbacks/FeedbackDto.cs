using Mapster;
using RoadMD.Domain.Entities;

namespace RoadMD.Application.Dto.Feedbacks
{
    public class FeedbackDto : IRegister
    {
        public Guid Id { get; init; }
        public string Subject { get; init; }
        public string Description { get; init; }
        public string UserName { get; init; }
        public string UserEmail { get; init; }

        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Feedback, FeedbackDto>();
        }
    }
}