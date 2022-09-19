using FluentValidation;
using RoadMD.Application.Dto.Feedbacks;

namespace RoadMD.Application.Validation.Feedback
{
    public class UpdateFeedbackValidator : AbstractValidator<UpdateFeedbackDto>
    {
        public UpdateFeedbackValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Subject)
                .NotEmpty()
                .MaximumLength(128);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(1024);

            RuleFor(x => x.UserName)
                .NotEmpty()
                .MaximumLength(128);

            RuleFor(x => x.UserEmail)
                .NotEmpty()
                .MaximumLength(128);
        }
    }
}