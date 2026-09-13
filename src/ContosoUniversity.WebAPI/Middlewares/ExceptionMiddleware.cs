using ContosoUniversity.WebAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                await WriteProblemDetailsAsync(
                    httpContext, StatusCodes.Status404NotFound, "Resource not found", ex.Message);
            }
            catch (ValidationException ex)
            {
                await WriteProblemDetailsAsync(
                    httpContext, StatusCodes.Status400BadRequest, "Validation error", ex.Message);
            }
            catch (DuplicateKeyException ex)
            {
                await WriteProblemDetailsAsync(
                    httpContext, StatusCodes.Status409Conflict, "Duplicate key error", ex.Message);
            }
            catch (DbUpdateConcurrencyException)
            {
                await WriteProblemDetailsAsync(
                    httpContext, StatusCodes.Status409Conflict, "Concurrency conflict",
                    "The record was modified or deleted by another user. Please reload the data and try again.");
            }
        }

        private static async Task WriteProblemDetailsAsync(
            HttpContext httpContext, int statusCode, string title, string detail)
        {
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";
            var problemDetails = new ProblemDetails
            {
                Title = title,
                Detail = detail,
                Status = statusCode,
                Instance = httpContext.Request.Path
            };
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            await httpContext.Response.WriteAsJsonAsync(problemDetails, jsonOptions);
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
