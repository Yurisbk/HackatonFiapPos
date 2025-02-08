using Domain.DTO;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);

            if ((context.Response.StatusCode < 200) || (context.Response.StatusCode > 299))
                await HandleExceptionAsync(context, $"[HTTP{context.Response.StatusCode}] {ReasonPhrases.GetReasonPhrase(context.Response.StatusCode)}", context.Response.StatusCode);
        }
        catch (ArgumentException ex)
        {
            await HandleExceptionAsync(context, ex.Message, StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, string message, int httpStatusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = httpStatusCode;
        var result = JsonSerializer.Serialize(new ApiError(message));
        return context.Response.WriteAsync(result);
    }
}
