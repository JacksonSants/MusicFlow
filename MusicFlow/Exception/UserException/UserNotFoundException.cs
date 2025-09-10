namespace MusicFlow.Exception.UserException;

public class UserNotFoundException : UserException
{
    public UserNotFoundException() { }
    public UserNotFoundException(string message) : base(message)
    {
    }
    public UserNotFoundException(string message, System.Exception innerException) : base(message, innerException)
    {
    }
}
