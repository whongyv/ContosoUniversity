using ContosoUniversity.WebAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ContosoUniversity.WebAPI.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (NotFoundException ex)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                httpContext.Response.ContentType = "application/json";
                var problemDetails = new ProblemDetails
                {
                    Title = "Source not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound,
                    Instance = httpContext.Request.Path
                };
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                await httpContext.Response.WriteAsJsonAsync(problemDetails, jsonOptions);
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
