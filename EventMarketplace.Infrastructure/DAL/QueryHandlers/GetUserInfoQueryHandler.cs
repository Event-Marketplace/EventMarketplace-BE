
using EventMarketplace.Application.Queries;
using EventMarketplace.Application.Response.UserResponse;
using EventMarketplace.Domain.Repositories;
using MediatR;

namespace EventMarketplace.Infrastructure.DAL.QueryHandlers;

public sealed class GetUserInfoQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserInfoQuery, GetUserInfoResponse>
{
    public async Task<GetUserInfoResponse> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByEmailAsync(request.Email) ?? throw new ApplicationException("Nie znaleziono użytkownika o danym adresie e-mail");

        return new GetUserInfoResponse()
        {
            Id = user.Id,
            Email = user.EmailAddress.Value,
            FirstName = user.FullName.FirstName,
            LastName = user.FullName.LastName,
            PhoneNumber = user.PhoneNumber.PhoneValue,
            Street = user.Address.Street,
            Number = user.Address.Number,
            City = user.Address.City,
            PostalCode = user.Address.PostalCode,
            RegistrationDate = user.CreatedAt
        };
    }
}