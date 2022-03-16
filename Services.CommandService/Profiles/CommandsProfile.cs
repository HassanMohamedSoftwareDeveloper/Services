using AutoMapper;
using Services.CommandService.Dtos;
using Services.CommandService.Models;
using Services.PlatformService;

namespace Services.CommandService.Profiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<Command, CommandReadDto>();
        CreateMap<CommandCreateDto, Command>();

        CreateMap<PlatformPublishedDto, Platform>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

        CreateMap<GrpcPlatformModel, Platform>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.PlatformId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Commands, opt => opt.Ignore());
    }
}
