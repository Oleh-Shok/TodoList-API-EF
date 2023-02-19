using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ToDoListApi.Models.Exceptions;

namespace ToDoListApi.Controllers
{
    public class HandlerErrorController : ControllerBase
    {
        private readonly IStringLocalizer _resourceLocalizer;
        public HandlerErrorController(IStringLocalizer resourceLocalizer)
        {
            _resourceLocalizer = resourceLocalizer;
        }

        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/error-development")]
        public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
        {
            if (!hostEnvironment.IsDevelopment())
            {
                throw new NotFoundException("Your environment is not development. Please recheck.");
            }
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;
            return Problem(detail: exceptionHandlerFeature.Error.StackTrace, title: exceptionHandlerFeature.Error.Message);
        }

        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("error")]
        public IResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error;
            IResult result = exception switch
            {
                ValidationException validationError => Results.Problem(statusCode: 400, title: _resourceLocalizer[validationError.Message], extensions: new Dictionary<string, object?> { ["trace-id"] = Activity.Current?.Id }),
                UnauthorizedException unauthorized => Results.Problem(statusCode: 401, title: unauthorized.Message, extensions: new Dictionary<string, object?> { ["trace-id"] = Activity.Current?.Id }),
                NotFoundException notFound => Results.Problem(statusCode: 404, title: notFound.Message, extensions: new Dictionary<string, object?> { ["trace-id"] = Activity.Current?.Id }),
                _ => Results.Problem(extensions: new Dictionary<string, object?> { ["trace-id"] = Activity.Current?.Id }),
            };
            return result;
        }
    }
}
