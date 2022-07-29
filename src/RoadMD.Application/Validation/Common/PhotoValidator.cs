using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace RoadMD.Application.Validation.Common
{
    public class PhotoValidator : AbstractValidator<IFormFile>
    {
        public PhotoValidator()
        {
            RuleFor(x => x).Must(HaveSupportedExtension)
                .WithMessage((_, i) => $"{Path.GetExtension(i.FileName)} is not a valid photo extension");
        }

        private static bool HaveSupportedExtension(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName);

            return AllowedExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase);
        }

        private static readonly string[] AllowedExtensions = {
            ".jpeg", ".jpg", ".png", ".bmp"
        };
    }
}