namespace noCarbon.Core.Exceptions;

public abstract class CustomException : Exception
{
    public CustomException()
    {
    }

    public CustomException(string message)
        : base(message)
    {
    }

    public CustomException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
