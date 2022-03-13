using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.PlatformService.Data;
using Services.PlatformService.Dtos;
using Services.PlatformService.Models;

namespace Services.PlatformService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo _platformRepo;
    private readonly IMapper _mapper;
    public PlatformsController(IPlatformRepo platformRepo, IMapper mapper)
    {
        _platformRepo = platformRepo;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        Console.WriteLine("--> Getting Platforms...");
        var platformItems = _platformRepo.GetAllPlatforms();
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
    }
    [HttpGet("{id:int}",Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
        Console.WriteLine($"--> Getting Platform By Id '{id}'...");
        var platformItem = _platformRepo.GetPlatformById(id);
        if (platformItem is null) return NotFound();
        return Ok(_mapper.Map<PlatformReadDto>(platformItem));
    }
    [HttpPost]
    public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto createDto)
    {
        var platform = _mapper.Map<Platform>(createDto);
        _platformRepo.CreatePlatform(platform);
        _platformRepo.SaveChanges();
        var platformReadDto=_mapper.Map<PlatformReadDto>(platform);
        return CreatedAtRoute(nameof(GetPlatformById), new { platform.Id },platformReadDto);
    }
}
