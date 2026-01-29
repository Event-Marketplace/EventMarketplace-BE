namespace EventMarketplace.Application.Exceptions;

public class EmUnauthorizeException(string message): EmException(message,"USER_NOT_AUTHENTICATED")
{
    
}