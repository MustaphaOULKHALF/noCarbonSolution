namespace noCarbon.API.Infrastracture;

/// <summary>
/// Generic response of the API
/// </summary>
/// <typeparam name="T"></typeparam>
public class Response<T>
{
    /// <summary>
    /// Get or set data from the API
    /// </summary>
    public T Data { get; set; }
    /// <summary>
    /// Get or set if the call was successfully executed
    /// </summary>
    public bool Succeeded { get; set; }
    /// <summary>
    /// Get or set message of the error/ success
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// set the message error
    /// </summary>
    /// <param name="errorMessage">the message error</param>
    /// <returns>Get response with the specific error</returns>
    public static Response<T> Fail(string errorMessage)
    {
        return new Response<T> { Succeeded = false, Message = errorMessage };
    }
    /// <summary>
    /// Successful operation
    /// </summary>
    /// <returns>Get response with successful state</returns>
    public static Response<T> Success()
    {
        return new Response<T> { Succeeded = true, Message = "Successful operation" };
    }
}
