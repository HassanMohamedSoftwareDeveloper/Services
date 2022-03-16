using Services.CommandService.Models;

namespace Services.CommandService.SyncDataServices.Grpc;

public interface IPlatformDataClient
{
    IEnumerable<Platform> ReturnAllPlatforms();
}
