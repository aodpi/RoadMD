using System.ComponentModel.DataAnnotations;

namespace RoadMD.Application.Dto.Vehicle
{
    public class VehicleDto
    {
        public Guid Id { get; set; }
        public string LetterCode { get; set; }
        public string NumberCode { get; set; }
    }

    public class CreateVehicleDto
    {
        [Required]
        public string LetterCode { get; set; }

        [Required]
        public string NumberCode { get; set; }
    }
}
