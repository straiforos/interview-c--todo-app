using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskManagement.Api.Interceptors;

/// <summary>
/// Action filter for mapping domain exceptions to standardized REST responses.
/// </summary>
public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        var exception = context.Exception;

        switch (exception)
        {
            case NotFoundException:
                context.Result = new NotFoundObjectResult(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "The requested resource was not found.",
                    Detail = exception.Message
                });
                context.ExceptionHandled = true;
                break;

            case ForbiddenException:
                context.Result = new ObjectResult(new ProblemDetails
                {
                    Status = StatusCodes.Status403Forbidden,
                    Title = "Forbidden",
                    Detail = exception.Message
                }) { StatusCode = StatusCodes.Status403Forbidden };
                context.ExceptionHandled = true;
                break;

            case BadRequestException:
                context.Result = new BadRequestObjectResult(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Bad Request",
                    Detail = exception.Message
                });
                context.ExceptionHandled = true;
                break;
        }
    }
}
