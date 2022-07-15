using LanguageExt;
using LanguageExt.Common;
using RoadMD.Application.Dto.Common;
using RoadMD.Application.Dto.InfractionCategory;

namespace RoadMD.Application.Services.InfractionCategories
{
    public interface IInfractionCategoriesService
    {
        Task<Result<InfractionCategoryDto>> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<LookupDto>> GetSelectListAsync(CancellationToken cancellationToken = default);

        Task<Result<InfractionCategoryDto>> CreateAsync(CreateInfractionCategoryDto createInfractionCategory,
            CancellationToken cancellationToken = default);

        Task<Result<InfractionCategoryDto>> UpdateAsync(UpdateInfractionCategoryDto updateInfractionCategory,
            CancellationToken cancellationToken = default);

        Task<Result<Unit>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}