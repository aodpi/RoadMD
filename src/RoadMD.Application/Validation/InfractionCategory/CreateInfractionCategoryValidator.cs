using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RoadMD.Application.Dto.InfractionCategory;
using RoadMD.EntityFrameworkCore;

namespace RoadMD.Application.Validation.InfractionCategory
{
    public class CreateInfractionCategoryValidator : AbstractValidator<CreateInfractionCategoryDto>
    {
        private readonly ApplicationDbContext _context;

        public CreateInfractionCategoryValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(150)
                .MustAsync((_, name, cancellationToken) => HaveUniqueNameAsync(name, cancellationToken))
                .When(c => !string.IsNullOrEmpty(c.Name));
        }

        private async Task<bool> HaveUniqueNameAsync(string name, CancellationToken cancellationToken)
        {
            return !await _context.InfractionCategories
                .AsNoTracking()
                .AnyAsync(c => c.Name.ToUpper().Equals(name.ToUpper()), cancellationToken);
        }
    }
}