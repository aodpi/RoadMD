using FluentValidation;
using RoadMD.Application.Dto.Vehicle;
using RoadMD.EntityFrameworkCore;

namespace RoadMD.Application.Validation.Vehicle
{
    public class CreateVehicleValidator : AbstractValidator<CreateVehicleDto>
    {
        private readonly ApplicationDbContext _context;

        public CreateVehicleValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(f => f.LetterCode)
                .NotEmpty()
                .WithMessage("Please provide the letter code");

            RuleFor(f => f.NumberCode)
                .NotEmpty()
                .WithMessage("Please provide number code")
                .When(f => !string.IsNullOrEmpty(f.LetterCode))
                .Must(HaveUniqueNumber)
                .WithMessage("A vehicle with the same number already exists");
        }

        private bool HaveUniqueNumber(CreateVehicleDto dto, string numberCode)
        {
            return !_context.Vehicles.Any(f => f.NumberCode == numberCode && f.LetterCode == dto.LetterCode);
        }
    }
}
