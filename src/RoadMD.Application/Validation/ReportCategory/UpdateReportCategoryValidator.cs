using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RoadMD.Application.Dto.InfractionCategories;
using RoadMD.EntityFrameworkCore;

namespace RoadMD.Application.Validation.ReportCategory
{
    public class UpdateReportCategoryValidator : AbstractValidator<UpdateInfractionCategoryDto>
    {
        private readonly ApplicationDbContext _context;

        public UpdateReportCategoryValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(150);

            When(dto => !string.IsNullOrEmpty(dto.Name), () =>
            {
                RuleFor(x => x.Name)
                    .Must((dto, name) => HaveUniqueName(dto.Id, name))
                    .WithMessage("A report category with the same {PropertyName} already exists.");
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