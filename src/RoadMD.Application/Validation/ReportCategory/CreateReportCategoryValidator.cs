﻿using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RoadMD.Application.Dto.ReportCategories;
using RoadMD.EntityFrameworkCore;

namespace RoadMD.Application.Validation.ReportCategory
{
    public class CreateReportCategoryValidator : AbstractValidator<CreateReportCategoryDto>
    {
        private readonly ApplicationDbContext _context;

        public CreateReportCategoryValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(150);

            When(dto => !string.IsNullOrEmpty(dto.Name), () =>
            {
                RuleFor(x => x.Name)
                    .Must(HaveUniqueName)
                    .WithMessage("A report category with the same {PropertyName} already exists.");
            });
        }

        private bool HaveUniqueName(string name)
        {
            return !_context.ReportCategories
                .AsNoTracking()
                .Any(c => c.Name.ToUpper().Equals(name.ToUpper()));
        }
    }
}