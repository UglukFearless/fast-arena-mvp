namespace FastArena.Core.Domain.Heroes;

/// <summary>
/// Template of the inventory slots a creature has, apart from the body zones.
/// </summary>
public class SlotLayout
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public List<SlotLayoutEntry>? Entries { get; set; }
}
