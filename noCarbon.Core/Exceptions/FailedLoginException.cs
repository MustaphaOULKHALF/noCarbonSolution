namespace noCarbon.Core.Exceptions;

public sealed class FailedLoginException : CustomException
{
    public FailedLoginException()
    {

    }

    public FailedLoginException(string message)
        : base(message)
    {
    }

    public FailedLoginException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
