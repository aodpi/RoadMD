﻿using FluentValidation;
using RoadMD.Application.Dto.Infractions.Update;

namespace RoadMD.Application.Validation.Infraction
{
    public class UpdateInfractionValidator : AbstractValidator<UpdateInfractionDto>
    {
        public UpdateInfractionValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Description)
                .MaximumLength(1024);

            RuleFor(x => x.CategoryId)
                .NotEmpty();

            RuleFor(x => x.Location)
                .NotEmpty();

            RuleFor(x => x.Vehicle)
                .NotEmpty();

            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Vehicle.Number)
                .NotEmpty()
                .MaximumLength(10);
        }
    }
}