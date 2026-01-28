namespace EventMarketplace.Application.Exceptions;

public class EmNotFoundException(string message) : EmAppException(message, "NOT_FOUND")
{
    
}