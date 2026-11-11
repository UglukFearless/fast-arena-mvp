namespace FastArena.Core.Domain.Heroes;

/// <summary>
/// Container of the hero inventory. The slot key is the slug of the slot: the zone slug for a body slot,
/// a dedicated key for a hand or a pocket. The collection follows from the key prefix.
/// </summary>
public class HeroSlot
{
    public Guid Id { get; set; }
    public Guid HeroId { get; set; }
    public required string SlotKey { get; set; }
    public Guid? HeroItemCellId { get; set; }

    public Hero? Hero { get; set; }
    public HeroItemCell? HeroItemCell { get; set; }
}
