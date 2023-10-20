using System.Net;
using Newtonsoft.Json;

namespace CustomManagementDemo.API.Extensions.Middlewares;

/// <summary>
/// 
/// </summary>
public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="next"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public GlobalExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
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
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var message = "Internal Server Error";

        switch (exception)
        {
            case ArgumentException argumentException:
                statusCode = (int)HttpStatusCode.BadRequest;
                message = $"Bad Request: {argumentException.Message}";
                break;

            case NotFoundException notFoundException:
                statusCode = (int)HttpStatusCode.NotFound;
                message = $"Not Found: {notFoundException.Message}";
                break;

            case InvalidOperationException invalidOperationException:
                statusCode = (int)HttpStatusCode.BadRequest;
                message = $"Invalid Operation: {invalidOperationException.Message}";
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var errorResponse = new ErrorResponse
        {
            StatusCode = statusCode,
            Message = message,
            ExceptionMessage = exception.Message,
            // Include additional details in the error response if needed
        };

        var jsonErrorResponse = JsonConvert.SerializeObject(errorResponse);
        return context.Response.WriteAsync(jsonErrorResponse);
    }
}

/// <summary>
/// 
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Represent the status code of the error 
    /// </summary>
    public int StatusCode { get; set; }
    
    /// <summary>
    /// Represent the message of the error
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// Represent the exception message of the error
    /// </summary>
    public string ExceptionMessage { get; set; }
    // Add additional properties as needed
}

/// <summary>
/// 
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public NotFoundException(string message) : base(message) { }
}