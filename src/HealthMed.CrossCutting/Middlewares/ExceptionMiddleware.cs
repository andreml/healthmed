using HealthMed.Application.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace HealthMed.CrossCutting.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new ErrorViewModel()
        {
            Exception = exception.Message,
            StackTrace = exception.StackTrace,
            Messages = null!
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
