namespace RoadMD.Domain.Entities
{
    public abstract class BaseEntity<T> where T : struct
    {
        public virtual T Id { get; set; }
    }

    public abstract class BaseEntity : BaseEntity<Guid>
    {
    }
}