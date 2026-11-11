namespace FastArena.Dal.Entities;

public class BodyZoneDal
{
    public Guid Id { get; set; }
    public Guid BodyMapId { get; set; }
    public required string Slug { get; set; }
    public required string Name { get; set; }
    public int UnitDamage { get; set; }

    public BodyMapDal? BodyMap { get; set; }
}
