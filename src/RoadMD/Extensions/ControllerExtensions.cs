using FluentValidation;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using RoadMD.Application.Exceptions;

namespace RoadMD.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult ToOk<TResult, TContract>(this Result<TResult> result,
            Func<TResult, TContract> mapper)
        {
            return result.Match<IActionResult>(obj =>
            {
                var response = mapper(obj);
                return new OkObjectResult(response);
            }, exception =>
            {
                switch (exception)
                {
                    case ValidationException validationException:
                        return new BadRequestObjectResult(validationException);
                    case NotFoundException notFoundException:
                    {
                        var details = new ProblemDetails
                        {
                            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                            Title = "The specified resource was not found.",
                            Detail = notFoundException.Message
                        };
                        return new NotFoundObjectResult(details);
                    }
                    default:
                    {
                        var details = new ProblemDetails
                        {
                            Status = StatusCodes.Status500InternalServerError,
                            Title = "An error occurred while processing your request.",
                            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                            Detail = exception.Message
                        };

                        return new ObjectResult(details) { StatusCode = StatusCodes.Status500InternalServerError };
                    }
                }
            });
        }

        public static IActionResult ToNoContent<TResult>(this Result<TResult> result)
        {
            return result.Match<IActionResult>(_ => new NoContentResult(), exception =>
            {
                switch (exception)
                {
                    case ValidationException validationException:
                        return new BadRequestObjectResult(validationException);
                    case NotFoundException notFoundException:
                    {
                        var details = new ProblemDetails
                        {
                            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                            Title = "The specified resource was not found.",
                            Detail = notFoundException.Message
                        };
                        return new NotFoundObjectResult(details);
                    }
                    default:
                    {
                        var details = new ProblemDetails
                        {
                            Status = StatusCodes.Status500InternalServerError,
                            Title = "An error occurred while processing your request.",
                            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                            Detail = exception.Message
                        };

                        return new ObjectResult(details) { StatusCode = StatusCodes.Status500InternalServerError };
                    }
                }
            });
        }
    }
}