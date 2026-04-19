using FastArena.Core.Domain.Heroes;

namespace FastArena.WebApi.Dtos;

public class HeroPocketSlotDto
{
    public EquipmentSlotType Slot { get; set; }
    public HeroItemCellDto? Item { get; set; }
}