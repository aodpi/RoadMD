using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RoadMD.Application.Dto.InfractionCategories;
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
                .MaximumLength(150);

            When(dto => !string.IsNullOrEmpty(dto.Name), () =>
            {
                RuleFor(x => x.Name)
                    .Must(HaveUniqueName)
                    .WithMessage("An infraction category with the same {PropertyName} already exists.");
            });
        }

        private bool HaveUniqueName(string name)
        {
            return !_context.InfractionCategories
                .AsNoTracking()
                .Any(c => c.Name.ToUpper().Equals(name.ToUpper()));
        }
    }
}