namespace FastArena.Core.Domain.Bodies;

/// <summary>
/// Hit zone of a body. Owned by the body map; a slot of the hero body is created for its slug.
/// </summary>
public class BodyZone
{
    public Guid Id { get; set; }
    public Guid BodyMapId { get; set; }
    public required string Slug { get; set; }
    public required string Name { get; set; }
    public int UnitDamage { get; set; }

    public BodyMap? BodyMap { get; set; }
}
