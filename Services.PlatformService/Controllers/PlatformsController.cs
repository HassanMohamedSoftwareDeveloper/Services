using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.PlatformService.AsyncDataServices;
using Services.PlatformService.Data;
using Services.PlatformService.Dtos;
using Services.PlatformService.Models;
using Services.PlatformService.SyncDataServices.Http;

namespace Services.PlatformService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    #region Fields :
    private readonly IPlatformRepo _platformRepo;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMessageBusClient _messageBusClient;
    #endregion

    #region CTORS :
    public PlatformsController(IPlatformRepo platformRepo, IMapper mapper, 
        ICommandDataClient commandDataClient, IMessageBusClient messageBusClient)
    {
        _platformRepo = platformRepo;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
        _messageBusClient = messageBusClient;
    }
    #endregion

    #region Endpoints :
    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        Console.WriteLine("--> Getting Platforms...");
        var platformItems = _platformRepo.GetAllPlatforms();
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
    }
    [HttpGet("{id:int}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
        Console.WriteLine($"--> Getting Platform By Id '{id}'...");
        var platformItem = _platformRepo.GetPlatformById(id);
        if (platformItem is null) return NotFound();
        return Ok(_mapper.Map<PlatformReadDto>(platformItem));
    }
    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto createDto)
    {
        var platform = _mapper.Map<Platform>(createDto);
        _platformRepo.CreatePlatform(platform);
        _platformRepo.SaveChanges();
        var platformReadDto = _mapper.Map<PlatformReadDto>(platform);
        //Send Sync Message
        try
        {
            await _commandDataClient.SendPlatformToCommand(platformReadDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not send synchrounously: {ex.Message}");
        }
        //Send Async Message
        try
        {
            var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
            platformPublishedDto.Event = "Platform_Published";
            _messageBusClient.PublishNewPlatform(platformPublishedDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not send asynchrounously: {ex.Message}");
        }
        return CreatedAtRoute(nameof(GetPlatformById), new { platform.Id }, platformReadDto);
    }
    #endregion
}
