namespace FastArena.Dal.Entities;

public class SlotLayoutEntryDal
{
    public Guid SlotLayoutId { get; set; }
    public required string SlotKey { get; set; }
    public int Count { get; set; }

    public SlotLayoutDal? SlotLayout { get; set; }
}
