using Services.CommandService.Models;

namespace Services.CommandService.Data;

public class CommandRepo : ICommandRepo
{
    private readonly AppDbContext _context;

    public CommandRepo(AppDbContext context)
    {
        _context = context;
    }

    //Platforms
    public IEnumerable<Platform> GetAllPlatforms() => _context.Platforms;
    public void CreatePlatform(Platform platform)
    {
        if (platform is null) throw new ArgumentNullException(nameof(platform));
        _context.Platforms.Add(platform);
    }
    public bool PlatformExists(int platformId) => _context.Platforms.Any(p=>p.Id.Equals(platformId));
    public bool ExternalPlatformExists(int externalPlatformId) => _context.Platforms.Any(p=>p.ExternalId.Equals(externalPlatformId));

    //Commands
    public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        => _context.Commands.Where(c => c.PlatformId.Equals(platformId));
    public Command GetCommand(int platformId, int commandId)
        => _context.Commands.Where(c => c.Id.Equals(commandId) && c.PlatformId.Equals(platformId)).FirstOrDefault();
    public void CreateCommand(int platformId, Command command)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));
        command.PlatformId = platformId;
        _context.Commands.Add(command);
    }


    public bool SaveChanges() => _context.SaveChanges() >= 0;
}
