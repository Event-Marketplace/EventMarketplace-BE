using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace EventMarketplace.Infrastructure.Middleware;

public class ErrorMiddleware
{
    private readonly RequestDelegate _next;
        
    public ErrorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);
        
        if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest)
        {
            await HandleBadRequestAsync(context);
        }
    }

    private static Task HandleBadRequestAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        var response = new { error = $"Bad - request, nieprawidłowe dane." };
        var json = JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(json);
    }
}