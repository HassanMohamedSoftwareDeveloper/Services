using Services.PlatformService.Dtos;

namespace Services.PlatformService.SyncDataServices.Http;

public interface ICommandDataClient
{
    Task SendPlatformToCommand(PlatformReadDto plat);
}
