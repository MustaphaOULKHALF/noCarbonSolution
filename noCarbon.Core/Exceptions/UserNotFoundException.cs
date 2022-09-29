namespace noCarbon.Core.Exceptions;

public sealed class UserNotFoundException : CustomException
{
    public UserNotFoundException()
    {

    }

    public UserNotFoundException(string message)
        : base(message)
    {
    }

    public UserNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
