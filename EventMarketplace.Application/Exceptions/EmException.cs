namespace EventMarketplace.Application.Exceptions;

public class EmException(string message, string errorCode) : Exception(message)
{
    public string ErrorCode { get; } = errorCode;
}