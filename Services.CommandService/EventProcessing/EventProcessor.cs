using System.Text.Json;
using AutoMapper;
using Services.CommandService.Data;
using Services.CommandService.Dtos;
using Services.CommandService.Models;

namespace Services.CommandService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(IServiceScopeFactory scopeFactory,IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }
    public void ProcessEvent(string message)
    {
        var eventType = DetermineExvent(message);
        switch (eventType)
        {
            case EventType.PlatformPublished:
                AddPlatform(message);
                break;
            case EventType.Undetermind:
                break;
            default:
                break;
        }
    }

    private EventType DetermineExvent(string notificationMessage)
    {
        Console.WriteLine("--> Determining Event");
        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
        switch (eventType.Event)
        {
            case "Platform_Published":
                Console.WriteLine("--> Platform Published Event Detected");
                return EventType.PlatformPublished;
            default:
                Console.WriteLine("--> Could not determine the event type");
                return EventType.Undetermind;
        }
    }
    private void AddPlatform(string platformPublishedMessage)
    {
        using var scope = _scopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

        var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);
        try
        {
            var platform = _mapper.Map<Platform>(platformPublishedDto);
            if (repo.ExternalPlatformExists(platform.ExternalId))
                Console.WriteLine("--> Platform already exists...");
            else
            {
                repo.CreatePlatform(platform);
                repo.SaveChanges();
                Console.WriteLine("--> Platform added!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not add Pltform to DB {ex.Message}");
        }
    }
}
enum EventType
{
    PlatformPublished,
    Undetermind
}
