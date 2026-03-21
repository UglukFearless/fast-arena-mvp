namespace FastArena.Dal.Entities;

public class PortraitTagDal
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public ICollection<PortraitDal>? Portraits { get; set; }
}
