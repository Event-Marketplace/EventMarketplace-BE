namespace EventMarketplace.Application.Exceptions;

public class EmConflictException(string message, string errorCode) : Exception(message)
{
    public string ErrorCode { get; } = errorCode;
}