using Microsoft.AspNetCore.Mvc;

namespace SoporteSistema.Middleware;

public sealed class ApiExceptionMiddleware(
    RequestDelegate next,
    ILogger<ApiExceptionMiddleware> logger,
    IHostEnvironment environment)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception) when (!context.Response.HasStarted)
        {
            // Guardamos el detalle en el log, no en la respuesta de producción.
            logger.LogError(exception, "Error no controlado procesando {Method} {Path}. TraceId: {TraceId}",
                context.Request.Method,
                context.Request.Path,
                context.TraceIdentifier);

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";

            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Se produjo un error interno.",
                Detail = environment.IsDevelopment()
                    ? exception.Message
                    : "Se produjo un error inesperado. Inténtelo de nuevo más tarde.",
                Instance = context.Request.Path
            };
            problem.Extensions["traceId"] = context.TraceIdentifier;

            await context.Response.WriteAsJsonAsync(problem, cancellationToken: context.RequestAborted);
        }
    }
}
