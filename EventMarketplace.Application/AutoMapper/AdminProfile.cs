using AutoMapper;
using EventMarketplace.Application.Response.AdminResponse;
using EventMarketplace.Domain.SharedModels;

namespace EventMarketplace.Application.Mapper;

public class AdminProfile : Profile
{
    public AdminProfile()
    {
        CreateMap<AdminStats, StatsResponse>();
    }
}