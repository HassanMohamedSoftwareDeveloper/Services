using Services.PlatformService.Dtos;

namespace Services.PlatformService.AsyncDataServices;

public interface IMessageBusClient
{
    void PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
}
