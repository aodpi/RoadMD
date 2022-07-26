namespace RoadMD.Domain.Entities
{
    public class Vehicle : BaseEntity
    {
        public Vehicle()
        {
            Infractions = new HashSet<Infraction>();
        }
        public string Number { get; set; }

        public ICollection<Infraction> Infractions { get; set; }
    }
}