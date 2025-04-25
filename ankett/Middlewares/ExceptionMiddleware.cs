using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Beklenmeyen bir hata oluştu!");

            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = ex switch
        {
            ArgumentNullException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        var message = ex switch
        {
            ArgumentNullException => "Eksik bilgi gönderildi.",
            UnauthorizedAccessException => "Yetkisiz işlem.",
            KeyNotFoundException => "İlgili veri bulunamadı.",
            _ => "Beklenmeyen bir hata oluştu. Lütfen tekrar deneyin."
        };

        var result = JsonSerializer.Serialize(new
        {
            statusCode = context.Response.StatusCode,
            message
        });

        return context.Response.WriteAsync(result);
    }
}
