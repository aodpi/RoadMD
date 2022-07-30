using FluentValidation;
using RoadMD.Application.Dto.Vehicles;
using RoadMD.EntityFrameworkCore;

namespace RoadMD.Application.Validation.Vehicle
{
    public class CreateVehicleValidator : AbstractValidator<CreateVehicleDto>
    {
        private readonly ApplicationDbContext _context;

        public CreateVehicleValidator(ApplicationDbContext context)
        {
            _context = context;


            RuleFor(f => f.Number)
                .NotEmpty()
                .WithMessage("Please provide number")
                .MaximumLength(10)
                .When(f => !string.IsNullOrEmpty(f.Number))
                .Must(HaveUniqueNumber)
                .WithMessage("A vehicle with the same number already exists");
        }

        private bool HaveUniqueNumber(CreateVehicleDto dto, string number)
        {
            return !_context.Vehicles.Any(f => f.Number.Equals(number));
        }
    }
}