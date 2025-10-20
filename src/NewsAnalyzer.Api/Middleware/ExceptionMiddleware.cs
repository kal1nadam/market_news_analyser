using System.Net;
using Microsoft.AspNetCore.Mvc;
using ValidationException = FluentValidation.ValidationException;

namespace NewsAnalyzer.Api.Middleware;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException vex)
        {
            var problem = new ProblemDetails()
            {
                Title = "Validation failed",
                Detail = string.Join("; ", vex.Errors.Select(f => $"{f.PropertyName} {f.ErrorMessage}")),
                Status = StatusCodes.Status400BadRequest
            };

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(problem);
        }
        catch (Exception ex)
        {
            var problem = new ProblemDetails()
            {
                Title = "Unhandled error",
                Detail = ex.Message,
                Status = StatusCodes.Status500InternalServerError,
            };
            
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}