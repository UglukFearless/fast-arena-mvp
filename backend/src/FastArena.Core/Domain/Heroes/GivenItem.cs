using FastArena.Core.Domain.Items;

namespace FastArena.Core.Domain.Heroes;

public class GivenItem
{
    public required Item Item {get; set; }
    public int Amount { get; set; } = 1;
}
