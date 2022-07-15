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
                .Must((dto, name) => HaveUniqueName(dto.Id, name))
                .When(c => !string.IsNullOrEmpty(c.Name));
        }

        private bool HaveUniqueName(Guid idToExclude, string name)
        {
            return !_context.InfractionCategories
                .AsNoTracking()
                .Any(c => c.Name.ToUpper().Equals(name.ToUpper()) && !c.Id.Equals(idToExclude));
        }
    }
}