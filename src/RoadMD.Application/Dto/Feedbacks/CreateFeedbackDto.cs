namespace RoadMD.Application.Dto.Feedbacks
{
    public class CreateFeedbackDto
    {
        public string Subject { get; init; }
        public string Description { get; init; }

        public string UserName { get; init; }
        public string UserEmail { get; init; }
    }
}