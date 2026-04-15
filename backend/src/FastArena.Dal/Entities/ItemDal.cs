using FastArena.Core.Domain.Items;

namespace FastArena.Dal.Entities;

public class ItemDal
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int BaseCost { get; set; }
    public required string ItemImage { get; set; }
    public required bool CanBeEquipped { get; set; }
    public required bool CanBeFolded { get; set; }
    public required ItemType Type { get; set; }

    public ICollection<HeroItemCellDal>? HeroItems { get; set; }
    public ICollection<EffectDefinitionDal>? Effects { get; set; }
    public ICollection<ItemAllowedSlotDal>? AllowedSlots { get; set; }
}
