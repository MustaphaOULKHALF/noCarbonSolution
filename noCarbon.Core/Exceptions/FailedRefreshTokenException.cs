namespace noCarbon.Core.Exceptions;

public sealed class FailedRefreshTokenException : CustomException
{
    public FailedRefreshTokenException()
    {

    }

    public FailedRefreshTokenException(string message)
        : base(message)
    {
    }

    public FailedRefreshTokenException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
