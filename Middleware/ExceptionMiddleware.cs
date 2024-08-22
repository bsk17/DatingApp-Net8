using System;
using System.Net;
using System.Text.Json;
using DatingAppServer.Errors;

namespace DatingAppServer.Middleware;

/// <summary>
/// In the request pipeline this Middleware will sit on top.
/// This Middleware will use the ApiException Model to create a JSON to return as response.
/// Also log the error if encoutered
/// </summary>
/// <param name="next">delegate to keep on moving request to the next ones</param>
/// <param name="logger">logger to log any exception</param>
/// <param name="env">to determine the current environment</param>
public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
{
    /// <summary>
    /// This method will check the current context.
    /// If only exception is caught, create the custom error based on environment other wise move onto next request.
    /// Transform into json.
    /// </summary>
    /// <param name="context">current http context to catch the error</param>
    /// <returns>Json format of custom error</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = env.IsDevelopment()
                ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace)
                : new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error !!");

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}
