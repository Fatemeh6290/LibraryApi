using System.Text.Json;
using LibraryApi.DTOs;

namespace LibraryApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;  //Speichert den nächsten Schritt der Pipeline
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);  //Gibt den Request an den nächsten Schritt weiter
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occured.");
            
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var error = new ErrorDto
            {
                Message = "An unexpected error occurred."
            };
            
            var json = JsonSerializer.Serialize(error);

            await context.Response.WriteAsync(json);
        }
    }
}