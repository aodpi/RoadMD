namespace RoadMD.Application.Dto.Infraction.Update
{
    [Serializable]
    public class UpdateInfractionLocationDto
    {
        public float Latitude { get; init; }
        public float Longitude { get; init; }
    }
}