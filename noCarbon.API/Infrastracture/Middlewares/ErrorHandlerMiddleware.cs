using noCarbon.Core.Exceptions;
using System.Net;
using System.Text.Json;

namespace noCarbon.API.Infrastracture.Middlewares;

/// <summary>
/// Get the error handler middleware
/// </summary>
public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    /// <summary>
    /// Ctor 
    /// </summary>
    /// <param name="next">Request delegate</param>
    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    /// <summary>
    /// add specific treatment to the pipeline
    /// </summary>
    /// <param name="context">the current context</param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = Response<string>.Fail(error.Message);
            switch (error)
            {
                case SuccessfulOperationException:
                    responseModel = Response<string>.Success();
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.OK;
                    break;
                case CustomException:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            var result = JsonSerializer.Serialize(responseModel);
            await response.WriteAsync(result);
        }
    }
}
