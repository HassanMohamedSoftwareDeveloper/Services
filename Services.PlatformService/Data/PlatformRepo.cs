using Services.PlatformService.Models;

namespace Services.PlatformService.Data;

public class PlatformRepo : IPlatformRepo
{
    private readonly AppDbContext _dbContext;

    public PlatformRepo(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }
    public void CreatePlatform(Platform platform)
    {
        if(platform is null) throw new ArgumentNullException(nameof(platform));
        _dbContext.Add(platform);
    }

    public IEnumerable<Platform> GetAllPlatforms() => _dbContext.Platforms;

    public Platform GetPlatformById(int id) => _dbContext.Platforms.Where(x => x.Id.Equals(id)).FirstOrDefault();

    public bool SaveChanges() => _dbContext.SaveChanges() >= 0;
}
