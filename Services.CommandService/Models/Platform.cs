using System.ComponentModel.DataAnnotations;

namespace Services.CommandService.Models;

public class Platform
{
    public Platform()
    {
        Commands = new HashSet<Command>();
    }
    [Required]
    [Key]
    public int Id { get; set; }
    [Required]
    public int ExternalId { get; set; }
    [Required]
    public string Name { get; set; }
    public ICollection<Command> Commands { get; set; }
}
