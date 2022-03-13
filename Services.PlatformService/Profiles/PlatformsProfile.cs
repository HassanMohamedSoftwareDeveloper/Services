using AutoMapper;
using Services.PlatformService.Dtos;
using Services.PlatformService.Models;

namespace Services.PlatformService.Profiles;

public class PlatformsProfile : Profile
{
    public PlatformsProfile()
    {
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<PlatformCreateDto, Platform>();
    }
}
