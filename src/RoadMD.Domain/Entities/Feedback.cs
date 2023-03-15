namespace RoadMD.Domain.Entities
{
    public class Feedback : BaseEntity
    {
        public string Subject { get; set; } = null!;
        public string Description { get; set; } = null!;

        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
    }
}