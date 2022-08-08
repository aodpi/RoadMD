using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RoadMD.Application.Dto.InfractionCategories;
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
                .When(c => !string.IsNullOrEmpty(c.Name));


            When(dto => !string.IsNullOrEmpty(dto.Name), () =>
            {
                RuleFor(x => x.Name)
                    .Must((dto, name) => HaveUniqueName(dto.Id, name))
                    .WithMessage("An infraction category with the same {PropertyName} already exists.");
            });
        }

        private bool HaveUniqueName(Guid idToExclude, string name)
        {
            return !_context.InfractionCategories
                .AsNoTracking()
                .Any(c => c.Name.ToUpper().Equals(name.ToUpper()) && !c.Id.Equals(idToExclude));
        }
    }
}