using AutoMapper;
using EventMarketplace.Application.Response.UserResponse;
using EventMarketplace.Domain.Entities;

namespace EventMarketplace.Application.Mapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, GetUserInfoResponse>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailAddress.Value))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FullName.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.FullName.LastName))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber.PhoneValue))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Address.Number))
            .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.CreatedAt));
    }
}