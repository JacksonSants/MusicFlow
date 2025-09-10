namespace MusicFlow.Exception.UserException;

public class UserException : System.Exception
{
    public UserException() { }
    public UserException(string message) : base(message)
    {
    }
    public UserException(string message, System.Exception innerException) : base(message, innerException) { }
}
