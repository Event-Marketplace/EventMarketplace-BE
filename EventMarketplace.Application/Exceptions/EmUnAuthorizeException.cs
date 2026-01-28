namespace EventMarketplace.Application.Exceptions;

public class EmUnAuthorizeException(string message): EmAppException(message,"USER_NOT_AUTHENTICATED")
{
    
}