using RoadMD.Application.Dto.Common;
using RoadMD.Application.Dto.InfractionCategory;

namespace RoadMD.Application.Services.InfractionCategories
{
    public interface IInfractionCategoriesService
    {
        Task<List<LookupDto>> GetSelectListAsync(CancellationToken cancellationToken = default);

        Task<InfractionCategoryDto> CreateAsync(CreateInfractionCategoryDto createInfractionCategory,
            CancellationToken cancellationToken = default);

        Task<InfractionCategoryDto> UpdateAsync(UpdateInfractionCategoryDto updateInfractionCategory,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}