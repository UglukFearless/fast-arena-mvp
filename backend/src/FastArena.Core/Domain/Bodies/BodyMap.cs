namespace FastArena.Core.Domain.Bodies;

/// <summary>
/// Template of a creature body: the hit zones the body has. The slug names the body and prefixes its zone keys.
/// </summary>
public class BodyMap
{
    public Guid Id { get; set; }
    public required string Slug { get; set; }
    public required string Name { get; set; }
    public List<BodyZone>? Zones { get; set; }
}
