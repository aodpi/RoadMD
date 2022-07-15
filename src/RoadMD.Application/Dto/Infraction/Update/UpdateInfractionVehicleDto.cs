namespace RoadMD.Application.Dto.Infraction.Update
{
    [Serializable]
    public class UpdateInfractionVehicleDto
    {
        public string LetterCode { get; init; }
        public string NumberCode { get; init; }
    }
}