namespace Olymp.Infrastructure;

public class BusinessLogicException : Exception
{
    public int StatusCode { get; set; }
    public BusinessLogicException(string message, int statusCode) : base(message)
    {
        
    }
}