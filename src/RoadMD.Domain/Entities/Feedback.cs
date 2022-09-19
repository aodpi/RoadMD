namespace RoadMD.Domain.Entities
{
    public class Feedback : BaseEntity
    {
        public string Subject { get; set; }
        public string Description { get; set; }

        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
}