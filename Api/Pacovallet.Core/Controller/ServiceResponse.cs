using System.Net;

namespace Pacovallet.Core.Controller;

public class ServiceResponse
{
    public bool Success { get; protected set; }
    public string? Message { get; protected set; }
    public HttpStatusCode StatusCode { get; protected set; }

    protected ServiceResponse(bool success, HttpStatusCode statusCode, string? message = null)
    {
        Success = success;
        Message = message;
        StatusCode = statusCode;
    }

    public static ServiceResponse Ok(string? message = null)
        => new(true, HttpStatusCode.OK, message);

    public static ServiceResponse Fail(string message, HttpStatusCode statusCode)
        => new(false, statusCode, message );

}

public class ServiceResponse<T> : ServiceResponse
{
    public T? Data { get; private set; }

    private ServiceResponse(
        bool success,
        T? data,
        string? message,
        HttpStatusCode statusCode)
        : base(success, statusCode, message)
    {
        Data = data;
    }

    public static ServiceResponse<T> Ok(
        T data,
        string? message = null)
        => new(true, data, message, HttpStatusCode.OK);

    public static ServiceResponse<T> Fail(
        string message,
        HttpStatusCode statusCode)
        => new(false, default, message, statusCode);
}
