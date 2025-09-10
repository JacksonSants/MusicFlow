namespace MusicFlow.Exception.UserException;

public class UserValidationException : System.Exception
{
    public UserValidationException() { }
    public UserValidationException(string message) : base(message) { }
    public UserValidationException(string message, System.Exception innerException) : base(message, innerException) { }
}
