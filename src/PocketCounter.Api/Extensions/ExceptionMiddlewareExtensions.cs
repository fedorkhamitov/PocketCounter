using PocketCounter.Api.Middleware;

namespace PocketCounter.Api.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    } 
}