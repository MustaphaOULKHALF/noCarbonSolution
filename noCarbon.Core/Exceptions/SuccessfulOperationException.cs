namespace noCarbon.Core.Exceptions;

public sealed class SuccessfulOperationException : Exception
{

    public SuccessfulOperationException()
    {
    }

    public SuccessfulOperationException(string message)
        : base(message)
    {
    }
}