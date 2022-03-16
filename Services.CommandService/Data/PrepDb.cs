using Services.CommandService.Models;
using Services.CommandService.SyncDataServices.Grpc;

namespace Services.CommandService.Data;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var grpcClient = serviceScope.ServiceProvider.GetRequiredService<IPlatformDataClient>();
        var platforms=grpcClient.ReturnAllPlatforms();
        SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>(),platforms);
    }
    private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
    {
        Console.WriteLine("--> Sedding new platforms...");
        foreach (var plat in platforms)
        {
            if (repo.ExternalPlatformExists(plat.ExternalId) is false)
                repo.CreatePlatform(plat);
        }
        repo.SaveChanges();
    }
}
