using FastArena.Core.Domain.Heroes;

namespace FastArena.WebApi.Profiles;

public static class HeroPocketSlotProfile
{
    public static Dtos.HeroPocketSlotDto Map(HeroSlot slot)
    {
        if (slot == null)
        {
            return null;
        }

        return new Dtos.HeroPocketSlotDto
        {
            Id = slot.Id,
            SlotKey = slot.SlotKey,
            Item = slot.HeroItemCell != null ? HeroItemCellProfile.Map(slot.HeroItemCell, true) : null,
        };
    }

    public static List<Dtos.HeroPocketSlotDto> MapList(List<HeroSlot> slots)
    {
        return slots?.ConvertAll(Map) ?? new List<Dtos.HeroPocketSlotDto>();
    }
}
