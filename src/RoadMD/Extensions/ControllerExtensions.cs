using FluentValidation;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using RoadMD.Application.Exceptions;

namespace RoadMD.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult ToOk<TResult>(this Result<TResult> result)
        {
            return result.Match(Ok, Fail);
        }

        public static IActionResult ToOk<TResult, TContract>(this Result<TResult> result,
            Func<TResult, TContract> mapper)
        {
            return result.Match(obj => Ok(mapper(obj)), Fail);
        }

        public static IActionResult ToNoContent<TResult>(this Result<TResult> result)
        {
            return result.Match(_ => new NoContentResult(), Fail);
        }

        private static IActionResult Ok<TResult>(TResult result)
        {
            return new OkObjectResult(result);
        }

        private static IActionResult Fail(Exception exception)
        {
            IActionResult GetActionResult(string type, string title, int statusCode, string detail)
            {
                var details = new ProblemDetails
                {
                    Type = type,
                    Title = title,
                    Status = statusCode,
                    Detail = detail
                };
                return new ObjectResult(details) { StatusCode = statusCode };
            }

            return exception switch
            {
                ValidationException validationException => new BadRequestObjectResult(validationException),
                NotFoundException notFoundException => GetActionResult(
                    "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    "The specified resource was not found.",
                    StatusCodes.Status404NotFound,
                    notFoundException.Message),
                ConflictException conflictException => GetActionResult(
                    "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.8",
                    "Request could not be completed",
                    StatusCodes.Status409Conflict,
                    conflictException.Message),
                _ => GetActionResult(
                    "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    "An error occurred while processing your request.",
                    StatusCodes.Status500InternalServerError,
                    exception.Message)
            };
        }
    }
}