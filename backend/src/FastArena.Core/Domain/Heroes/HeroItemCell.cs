using FastArena.Core.Domain.Items;

namespace FastArena.Core.Domain.Heroes;

public class HeroItemCell
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public Guid HeroId { get; set; }
    public int Amount { get; set; }

    public Hero? Hero { get; set; }
    public Item? Item { get; set; }
}
