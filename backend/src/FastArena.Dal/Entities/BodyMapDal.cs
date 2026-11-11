namespace FastArena.Dal.Entities;

public class BodyMapDal
{
    public Guid Id { get; set; }
    public required string Slug { get; set; }
    public required string Name { get; set; }

    public ICollection<BodyZoneDal>? Zones { get; set; }
}
