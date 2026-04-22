using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Api.Interceptors;

public static class ExceptionHandlingExtensions
{
    public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    context.Response.ContentType = "application/problem+json";
                    
                    var problemDetails = new ProblemDetails
                    {
                        Status = context.Response.StatusCode,
                        Title = "An error occurred while processing your request",
                        Detail = contextFeature.Error.Message,
                        Instance = context.Request.Path
                    };

                    // Customize status codes based on exception type if needed
                    // e.g., if (contextFeature.Error is NotFoundException) problemDetails.Status = 404;

                    await context.Response.WriteAsJsonAsync(problemDetails);
                }
            });
        });
    }
}
