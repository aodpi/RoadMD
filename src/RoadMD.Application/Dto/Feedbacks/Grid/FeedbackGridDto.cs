using Mapster;
using RoadMD.Domain.Entities;

namespace RoadMD.Application.Dto.Feedbacks.Grid
{
    public class FeedbackGridDto : IRegister
    {
        public Guid Id { get; init; }
        public string Subject { get; init; }
        public string UserEmail { get; init; }

        /// <inheritdoc />
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Feedback, FeedbackGridDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Subject, src => src.Subject)
                .Map(dest => dest.UserEmail, src => src.UserEmail)
                .IgnoreNonMapped(true);
        }
    }
}