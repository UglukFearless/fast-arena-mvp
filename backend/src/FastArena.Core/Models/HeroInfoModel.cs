using FastArena.Core.Domain.Heroes;

namespace FastArena.Core.Models;

public class HeroInfoModel
{
    public required Hero Hero { get; set; }
    public bool IsInventoryVisible { get; set; }
    public int MoneyAmount { get; set; }
    public List<HeroItemCell> InventoryItems { get; set; } = new List<HeroItemCell>();
}
