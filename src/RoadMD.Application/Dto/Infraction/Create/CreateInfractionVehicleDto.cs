namespace RoadMD.Application.Dto.Infraction.Create
{
    [Serializable]
    public class CreateInfractionVehicleDto
    {
        public string LetterCode { get; init; }
        public string NumberCode { get; init; }
    }
}