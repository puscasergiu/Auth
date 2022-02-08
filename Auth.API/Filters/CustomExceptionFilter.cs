using System.Linq;
using System.Net;
using Auth.API.Models;
using Auth.Core.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Auth.API.Filters
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<CustomExceptionFilter> _logger;

        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException validationException)
            {
                context.Result = new BadRequestObjectResult(
                    new ValidationErrorModel(
                        "Validation failed",
                        validationException.Errors.Select(s => new ValidationErrorDetailModel(s.PropertyName, s.ErrorMessage)).ToList()
                        ));
                return;
            }

            if (context.Exception is NotFoundException notFoundException)
            {
                context.Result = new NotFoundObjectResult(new DomainErrorModel(notFoundException.Message));
                return;
            }

            if (context.Exception is DomainException domainException)
            {
                context.Result = new BadRequestObjectResult(new DomainErrorModel(domainException.Message));
                return;
            }

            _logger.LogError($"An unhandled exception occurred while executing the request: {context.Exception}");
            context.Result = new ObjectResult("An unhandled exception occurred while executing the request")
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }
}
