using Microsoft.EntityFrameworkCore;
using Services.PlatformService.Models;

namespace Services.PlatformService.Data;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app,bool isProd)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
    }
    private static void SeedData(AppDbContext context,bool isProd)
    {
        if (isProd)
        {
            Console.WriteLine("--> Attempting to apply migrations...");
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Couldn't run migrations: {ex.Message}");
            }
        }
        if (context.Platforms.Any() is false)
        {
            Console.WriteLine("--> Sedding Data...");
            context.Platforms.AddRange
                (
                new Platform { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                new Platform { Name = "Sql Server Express", Publisher = "Microsoft", Cost = "Free" },
                new Platform { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                );

            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> We already have data");
        }
    }
}
