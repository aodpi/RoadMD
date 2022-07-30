namespace RoadMD.Application.Dto.InfractionCategories
{
    public class UpdateInfractionCategoryDto : CreateInfractionCategoryDto
    {
        public Guid Id { get; init; }
    }
}