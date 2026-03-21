namespace FastArena.Core.Domain;

public class PortraitTag
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public ICollection<Portrait>? Portraits { get; set; }
}
