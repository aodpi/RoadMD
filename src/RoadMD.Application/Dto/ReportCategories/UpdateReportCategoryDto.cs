namespace RoadMD.Application.Dto.ReportCategories
{
    public class UpdateReportCategoryDto : CreateReportCategoryDto
    {
        public Guid Id { get; init; }
    }
}