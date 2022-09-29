namespace noCarbon.Core.Exceptions;

public sealed class DataAlreadyExistException : CustomException
{
    public DataAlreadyExistException()
    {
    }

    public DataAlreadyExistException(string message)
        : base(message)
    {
    }

    public DataAlreadyExistException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
