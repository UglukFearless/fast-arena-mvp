namespace FastArena.Dal.Entities;

public class SlotLayoutDal
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public ICollection<SlotLayoutEntryDal>? Entries { get; set; }
}
