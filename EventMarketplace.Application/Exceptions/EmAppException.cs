namespace EventMarketplace.Application.Exceptions;

public class EmAppException(string message, string errorCode) : Exception(message)
{
    public string ErrorCode { get; } = errorCode;
}