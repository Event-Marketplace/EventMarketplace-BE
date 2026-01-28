using System.Net;
using System.Text.Json;
using EventMarketplace.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EventMarketplace.Infrastructure.Middleware;

public class ErrorMiddleware(RequestDelegate next, ILogger<ErrorMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (EmConflictException ex)
        {
            logger.LogWarning(ex, "Conflict state exception");
            await CatchErrors(context, StatusCodes.Status409Conflict, ex.ErrorCode, ex.Message);
        }
        catch (EmNotFoundException ex)
        {
            await Handle(context, StatusCodes.Status404NotFound, ex, "Not found exception");
        }
        catch (EmForbiddenException ex)
        {
            await Handle(context, StatusCodes.Status403Forbidden, ex, "Forbidden exception");
        } 
        catch (EmUnAuthorizeException ex)
        {
            await Handle(context, StatusCodes.Status401Unauthorized, ex, "Authorization / Authentication exception");
        }
        catch (EmAppException ex)
        {
            await Handle(context, StatusCodes.Status400BadRequest, ex, "Application error");
        }
        catch (FluentValidation.ValidationException ex)
        {
            await HandleValidationException(context, ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Nieobsłużony wyjątek");
            await CatchErrors(context, StatusCodes.Status500InternalServerError, "INTERNAL_ERROR", "Unexpected server error");}
    }
    
    private async Task HandleValidationException(
        HttpContext context,
        FluentValidation.ValidationException ex)
    {
        logger.LogWarning(ex, "Błędy walidacji");

        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";

        var response = new
        {
            errorCode = "VALIDATION_ERROR",
            errors = ex.Errors.Select(e => new
            {
                field = e.PropertyName,
                message = e.ErrorMessage
            })
        };

        await context.Response.WriteAsJsonAsync(response);
    }
    
    private async Task Handle(HttpContext context, int statusCode, EmAppException ex, string logMessage)
    {
        logger.LogWarning(ex, "{LogMessage} | ErrorCode={ErrorCode}", logMessage, ex.ErrorCode);
        await CatchErrors(context, statusCode, ex.ErrorCode, ex.Message);
    }


    private async Task CatchErrors(HttpContext httpContext, int statusCode, string errorCode, string message)
    {
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        var response = new { errorCode, message };
        await httpContext.Response.WriteAsJsonAsync(response);
    }
}