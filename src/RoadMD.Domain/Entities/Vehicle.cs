namespace RoadMD.Domain.Entities
{
    public class Vehicle : BaseEntity
    {
        public Vehicle()
        {
            Infractions = new HashSet<Infraction>();
        }

        public string LetterCode { get; set; }
        public string NumberCode { get; set; }

        public ICollection<Infraction> Infractions { get; set; }
    }
}