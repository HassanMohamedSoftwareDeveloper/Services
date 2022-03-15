using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.CommandService.Data;
using Services.CommandService.Dtos;
using Services.CommandService.Models;

namespace Services.CommandService.Controllers;
[Route("api/c/platforms/{platformId:int}/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepo _repository;
    private readonly IMapper _mapper;

    public CommandsController(ICommandRepo repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
    {
        Console.WriteLine($"--> Hit GetCommandsFroPlatform: {platformId}");
        if (_repository.PlatformExists(platformId) is false) return NotFound();

        var commandItems = _repository.GetCommandsForPlatform(platformId);
        return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
    }
    [HttpGet("{commandId:int}",Name = "GetCommandForPlatform")]
    public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId,int commandId)
    {
        Console.WriteLine($"--> Hit GetCommandForPlatform: {platformId} / {commandId}");
        if (_repository.PlatformExists(platformId) is false) return NotFound();
        var commandItem = _repository.GetCommand(platformId, commandId);
        if(commandItem is null) return NotFound();
        return Ok(_mapper.Map<CommandReadDto>(commandItem));
    }
    [HttpPost]
    public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
    {
        Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");
        if (_repository.PlatformExists(platformId) is false) return NotFound();
        var command = _mapper.Map<Command>(commandDto);
        _repository.CreateCommand(platformId, command);
        _repository.SaveChanges();
        return CreatedAtRoute(nameof(GetCommandForPlatform), new { platformId, commandId = command.Id }
        , _mapper.Map<CommandReadDto>(command));
    }
}
