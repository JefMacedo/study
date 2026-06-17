using System.Net;
using System.Text.Json;
using FluentValidation;

namespace TodoApp.Api.ExceptionHandling;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ValidationException vex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            httpContext.Response.ContentType = "application/json";
            var payload = JsonSerializer.Serialize(new { errors = vex.Errors });
            await httpContext.Response.WriteAsync(payload);
        }
        catch (Exception ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";
            var payload = JsonSerializer.Serialize(new { error = ex.Message });
            await httpContext.Response.WriteAsync(payload);
        }
    }
}
