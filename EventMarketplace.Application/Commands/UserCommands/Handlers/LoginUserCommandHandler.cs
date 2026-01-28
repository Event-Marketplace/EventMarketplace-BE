
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Response;
using EventMarketplace.Application.Utils;
using EventMarketplace.Application.Utils.Jwt;
using EventMarketplace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventMarketplace.Application.Commands.UserCommands.Handlers;

public class LoginUserCommandHandler(
    IUnitOfWork unitOfWork, 
    IPasswordManager passwordManager, 
    ILogger<LoginUserCommandHandler> logger,
    IJwtProvider jwtProvider
    ) : IRequestHandler<LoginUserCommand, JwtTokenResponse>
{
    public async Task<JwtTokenResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var userFromDb = await unitOfWork.Users.GetUserByEmailAsync(request.Dto.Email) ??
                         throw new EmNotFoundException($"User with email: {request.Dto.Email}, was not found!");

        if (!passwordManager.ValidPassword(request.Dto.Password, userFromDb.Password))
            throw new EmAppException("Given password is wrong!","INVALID_PASSWORD");

        var jwtToken = jwtProvider.GenerateToken(userFromDb);
        var refreshToken = jwtProvider.GenerateRefreshToken(userFromDb);
        
        jwtProvider.AppendRefreshToken(refreshToken.RefreshToken);
        
        var newRefreshToken =
            RefreshToken.Create(refreshToken.RefreshToken, refreshToken.Expires, false, userFromDb.Id);
        await unitOfWork.Auths.AddNewRefreshTokenAsync(newRefreshToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("User was logged successfully.");
        
        return new JwtTokenResponse()
        {
            TokenJwt = jwtToken
        };
    }
}