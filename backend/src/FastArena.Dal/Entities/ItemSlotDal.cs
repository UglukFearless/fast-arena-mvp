namespace FastArena.Dal.Entities;

public class ItemSlotDal
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public required string SlotKey { get; set; }

    public ItemDal? Item { get; set; }
}
