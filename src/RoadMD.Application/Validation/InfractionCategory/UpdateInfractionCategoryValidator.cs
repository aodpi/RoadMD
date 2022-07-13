using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RoadMD.Application.Dto.InfractionCategory;
using RoadMD.EntityFrameworkCore;

namespace RoadMD.Application.Validation.InfractionCategory
{
    public class UpdateInfractionCategoryValidator : AbstractValidator<UpdateInfractionCategoryDto>
    {
        private readonly ApplicationDbContext _context;

        public UpdateInfractionCategoryValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(150)
                .MustAsync((dto, name, cancellationToken) => HaveUniqueNameAsync(dto.Id, name, cancellationToken))
                .When(c => !string.IsNullOrEmpty(c.Name));
        }

        private async Task<bool> HaveUniqueNameAsync(Guid idToExclude, string name, CancellationToken cancellationToken)
        {
            return !await _context.InfractionCategories
                .AsNoTracking()
                .AnyAsync(c => c.Name.ToUpper().Equals(name.ToUpper()) && !c.Id.Equals(idToExclude), cancellationToken);
        }
    }
}