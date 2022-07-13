using System.ComponentModel.DataAnnotations;

namespace RoadMD.Domain.Entities
{
    public class Vehicle
    {
        [Key]
        public Guid Id { get; set; }
        public string LetterCode { get; set; }
        public string NumberCode { get; set; }
    }
}
