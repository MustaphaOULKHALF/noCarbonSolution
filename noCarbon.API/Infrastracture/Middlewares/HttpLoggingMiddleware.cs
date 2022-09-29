using System.Diagnostics;
using System.Text;

namespace noCarbon.API.Infrastracture.Middlewares;

/// <summary>
/// Middleware for Logging Request and Responses.
/// Remarks: Original code taken from https://exceptionnotfound.net/using-middleware-to-log-requests-and-responses-in-asp-net-core/
/// </summary>
public class HttpLoggingMiddleware
{
    #region Fields
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;
    #endregion

    #region Ctor
    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="next">Request delegate</param>
    /// <param name="logger">logger</param>
    public HttpLoggingMiddleware(RequestDelegate next, ILogger<HttpLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    #endregion
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context">current context</param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context)
    {
        var watch = new Stopwatch();
        watch.Start();
        StringBuilder builder = new(Environment.NewLine);
        //Copy  pointer to the original response body stream
        var originalBodyStream = context.Response.Body;

        //Add to log
        builder.AppendLine($"request : { await GetRequestAsTextAsync(context.Request)}");

        //Create a new memory stream and use it for the temp reponse body
        await using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        //Continue down the Middleware pipeline
        await _next(context);

        //Add to log
        builder.AppendLine($"response : {await GetResponseAsTextAsync(context.Response)}");

        //Copy the contents of the new memory stream, which contains the response to the original stream, which is then returned to the client.
        await responseBody.CopyToAsync(originalBodyStream);
        watch.Stop();
        builder.AppendLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        _logger.LogInformation(builder.ToString());
    }

    /// <summary>
    /// Get request as a string
    /// </summary>
    /// <param name="request">current httpRequest</param>
    /// <returns>request body</returns>
    protected virtual async Task<string> GetRequestAsTextAsync(HttpRequest request)
    {
        if (request.Method != "POST")
        {
            var body = request.Body;
            //Set the reader for the request back at the beginning of its stream.
            request.EnableBuffering();
            //Read request stream
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            //Copy into  buffer.
            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            //Convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            //Assign the read body back to the request body
            request.Body = body;

            return $"{request.Scheme} {request.Host} {request.Path} {request.QueryString} {bodyAsText}";
        }
        else
        {
            if (!request.Body.CanSeek)
            {
                // We only do this if the stream isn't *already* seekable,
                // as EnableBuffering will create a new stream instance
                // each time it's called
                request.EnableBuffering();
            }
            request.Body.Position = 0;
            var reader = new StreamReader(request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync().ConfigureAwait(false);
            request.Body.Position = 0;
            return $"{request.Scheme} {request.Host} {request.Path} {request.QueryString} {body}";
        }
    }
    /// <summary>
    /// Get response as a string
    /// </summary>
    /// <param name="response">current httpResponse</param>
    /// <returns>response body</returns>
    protected virtual async Task<string> GetResponseAsTextAsync(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        //Create stream reader to write entire stream
        var text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);

        return text;
    }
}