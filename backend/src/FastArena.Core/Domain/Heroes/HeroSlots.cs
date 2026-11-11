using FastArena.Core.Domain.Bodies;

namespace FastArena.Core.Domain.Heroes;

/// <summary>
/// Slot keys are namespaced by owner. A zone slot belongs to a body map: HUMANOID_BODY_HEAD.
/// Hands and pockets are shared until bodies need their own layout: HAND_LEFT, HAND_RIGHT, POCKET.
/// An item declares the keys it occupies.
/// </summary>
public static class SlotKeys
{
    public const string BodySeparator = "_BODY_";
    public const string LayoutSeparator = "_LAYOUT_";

    public const string Pocket = "POCKET";
    public const string RightHand = "HAND_RIGHT";
    public const string LeftHand = "HAND_LEFT";

    public static string Zone(BodyMap bodyMap, BodyZone zone) => bodyMap.Slug + BodySeparator + zone.Slug;

    public static bool IsHand(string slotKey) => slotKey is RightHand or LeftHand;

    public static bool IsPocket(string slotKey) => slotKey == Pocket;

    public static bool IsZone(string slotKey) => slotKey.Contains(BodySeparator, StringComparison.Ordinal);
}

public static class HeroSlots
{
    /// <summary>
    /// Materializes the hero slots: body slots follow the zones of the body map, the rest follow the layout.
    /// </summary>
    public static List<HeroSlot> Create(Guid heroId, BodyMap bodyMap, SlotLayout layout)
    {
        var slots = new List<HeroSlot>();

        foreach (var zone in bodyMap.Zones ?? new List<BodyZone>())
        {
            slots.Add(new HeroSlot
            {
                Id = Guid.NewGuid(),
                HeroId = heroId,
                SlotKey = SlotKeys.Zone(bodyMap, zone),
            });
        }

        foreach (var entry in layout.Entries ?? new List<SlotLayoutEntry>())
        {
            for (var i = 0; i < entry.Count; i++)
            {
                slots.Add(new HeroSlot
                {
                    Id = Guid.NewGuid(),
                    HeroId = heroId,
                    SlotKey = entry.SlotKey,
                });
            }
        }

        return slots;
    }
}
