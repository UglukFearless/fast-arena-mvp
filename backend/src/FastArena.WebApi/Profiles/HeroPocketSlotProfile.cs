using FastArena.Core.Domain.Heroes;
using FastArena.WebApi.Dtos;

namespace FastArena.WebApi.Profiles;

public static class HeroPocketSlotProfile
{
    public static HeroPocketSlotDto Map(HeroEquippedSlot slot)
    {
        if (slot == null)
        {
            return null;
        }

        return new HeroPocketSlotDto
        {
            Slot = slot.Slot,
            Item = slot.HeroItemCell != null ? HeroItemCellProfile.Map(slot.HeroItemCell, true) : null,
        };
    }

    public static List<HeroPocketSlotDto> Map(List<HeroEquippedSlot> slots)
    {
        return slots?.ConvertAll(Map) ?? new List<HeroPocketSlotDto>();
    }
}