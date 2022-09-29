namespace noCarbon.Core.Exceptions;

public sealed class EntityNotFoundException : CustomException
{
    public EntityNotFoundException()
    {
    }

    public EntityNotFoundException(string message)
        : base(message)
    {
    }

    public EntityNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
