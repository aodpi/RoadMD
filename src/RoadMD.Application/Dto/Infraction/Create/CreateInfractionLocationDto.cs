namespace RoadMD.Application.Dto.Infraction.Create
{
    [Serializable]
    public class CreateInfractionLocationDto
    {
        public float Latitude { get; init; }
        public float Longitude { get; init; }
    }
}