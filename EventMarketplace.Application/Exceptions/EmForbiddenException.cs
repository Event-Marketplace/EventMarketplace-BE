namespace EventMarketplace.Application.Exceptions;

public class EmForbiddenException(string message) : EmException(message, "FORBIDDEN")
{
    
}