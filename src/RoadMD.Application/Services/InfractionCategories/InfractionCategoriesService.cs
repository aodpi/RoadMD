using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using RoadMD.Application.Dto.Common;
using RoadMD.Application.Dto.InfractionCategory;
using RoadMD.Domain.Entities;
using RoadMD.EntityFrameworkCore;

namespace RoadMD.Application.Services.InfractionCategories
{
    public class InfractionCategoriesService : ServiceBase, IInfractionCategoriesService
    {
        public InfractionCategoriesService(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<List<LookupDto>> GetSelectListAsync(CancellationToken cancellationToken)
        {
            return await Context.InfractionCategories
                .OrderBy(x => x.Name)
                .Select(x => new LookupDto(x.Id, x.Name))
                .ToListAsync(cancellationToken);
        }

        public async Task<InfractionCategoryDto> CreateAsync(CreateInfractionCategoryDto createInfractionCategory,
            CancellationToken cancellationToken)
        {
            var entity = new InfractionCategory
            {
                Name = createInfractionCategory.Name
            };

            await Context.InfractionCategories.AddAsync(entity, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);

            return Mapper.Map<InfractionCategoryDto>(entity);
        }

        public async Task<InfractionCategoryDto> UpdateAsync(UpdateInfractionCategoryDto updateInfractionCategory,
            CancellationToken cancellationToken)
        {
            var entity = await Context.InfractionCategories
                .SingleOrDefaultAsync(x =>
                    x.Id.Equals(updateInfractionCategory.Id), cancellationToken: cancellationToken);

            entity.Name = updateInfractionCategory.Name;

            Context.InfractionCategories.Update(entity);
            await Context.SaveChangesAsync(cancellationToken);

            return Mapper.Map<InfractionCategoryDto>(entity);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await Context.InfractionCategories
                .SingleOrDefaultAsync(x =>
                    x.Id.Equals(id), cancellationToken: cancellationToken);

            Context.InfractionCategories.Remove(entity);
            await Context.SaveChangesAsync(cancellationToken);
        }
    }
}