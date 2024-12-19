namespace Dishes.Common.Exceptions;

public class BaseException : Exception
{
    public string? ExceptionType { get; protected set; }

    public string? Title { get; protected set; }

    public int? Status { get; protected set; }

    public string? Detail { get; protected set; }

    public BaseException(string message) : base(message) => ExceptionType = "unknown";

    public BaseException(string message, Exception innerException) : base(message, innerException)
        => ExceptionType = innerException.Message;

    public BaseException(
        string? type,
        string? title,
        int status,
        string? details
    ) : base(title)
    {
        ExceptionType = type;
        Title = title;
        Status = status;
        Detail = details;
    }

    public BaseException(
        string? type,
        string? title,
        int status,
        string? details,
        Exception? innerException
    ) : base(title, innerException)
    {
        ExceptionType = type;
        Title = title;
        Status = status;
        Detail = details;
    }
}
