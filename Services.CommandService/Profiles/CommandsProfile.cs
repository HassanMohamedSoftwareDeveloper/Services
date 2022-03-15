using AutoMapper;
using Services.CommandService.Dtos;
using Services.CommandService.Models;

namespace Services.CommandService.Profiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<Command, CommandReadDto>();
        CreateMap<CommandCreateDto, Command>();
    }
}
