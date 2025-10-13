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
        catch (AppException ex)
        {
            logger.LogWarning($"Błąd biznesowy: {ex.Message}");
            
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";
            
            var response = new { error = ex.Message };
            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
        catch (Exception ex)
        {
            logger.LogWarning($"Nieoczekiwany bład serwera - {ex.Message}");
            
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            
            var response = new { error = "Wystąpił nieoczekiwany błąd serwera" };
            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}