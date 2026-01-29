namespace EventMarketplace.Application.Exceptions;

public class EmNotFoundException(string message) : EmException(message, "NOT_FOUND")
{
    
}