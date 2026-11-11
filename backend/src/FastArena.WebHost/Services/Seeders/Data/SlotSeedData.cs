using FastArena.Core.Domain;
using FastArena.Core.Domain.Heroes;
using FastArena.Dal.Entities;

namespace FastArena.WebHost.Services.Seeders.Data;

public static class SlotSeedData
{
    public static readonly Guid HumanoidBodyMapId = Guid.Parse("b1f5b7c2-5c1e-4c8f-9a7e-2c1b3d4e5f60");
    public static readonly Guid HumanoidSlotLayoutId = Guid.Parse("a7c9d1e3-2b4f-4a6c-8d0e-1f3b5c7d9a20");

    private static readonly (Guid Id, HitZone Zone, string Name, int UnitDamage)[] HumanoidZones =
    {
        (Guid.Parse("5e0c3f11-6d2a-4b8f-9c31-7a4e1f2d8b01"), HitZone.HEAD, "Голова", 20),
        (Guid.Parse("5e0c3f11-6d2a-4b8f-9c31-7a4e1f2d8b02"), HitZone.LEFT_HAND, "Левая рука", 4),
        (Guid.Parse("5e0c3f11-6d2a-4b8f-9c31-7a4e1f2d8b03"), HitZone.RIGHT_HAND, "Правая рука", 4),
        (Guid.Parse("5e0c3f11-6d2a-4b8f-9c31-7a4e1f2d8b04"), HitZone.LEFT_LEG, "Левая нога", 5),
        (Guid.Parse("5e0c3f11-6d2a-4b8f-9c31-7a4e1f2d8b05"), HitZone.RIGHT_LEG, "Правая нога", 5),
        (Guid.Parse("5e0c3f11-6d2a-4b8f-9c31-7a4e1f2d8b06"), HitZone.NECK, "Горло", 25),
        (Guid.Parse("5e0c3f11-6d2a-4b8f-9c31-7a4e1f2d8b07"), HitZone.LEFT_CHEST, "Левое плечо", 15),
        (Guid.Parse("5e0c3f11-6d2a-4b8f-9c31-7a4e1f2d8b08"), HitZone.RIGHT_CHEST, "Правое плечо", 15),
        (Guid.Parse("5e0c3f11-6d2a-4b8f-9c31-7a4e1f2d8b09"), HitZone.STOMACH, "Живот", 10),
        (Guid.Parse("5e0c3f11-6d2a-4b8f-9c31-7a4e1f2d8b0a"), HitZone.GROIN, "Пах", 10),
        (Guid.Parse("5e0c3f11-6d2a-4b8f-9c31-7a4e1f2d8b0b"), HitZone.HEART, "Сердце", 30),
    };

    public static BodyMapDal HumanoidBodyMap => new()
    {
        Id = HumanoidBodyMapId,
        Slug = "HUMANOID",
        Name = "Гуманоид",
        Zones = HumanoidZones
            .Select(z => new BodyZoneDal
            {
                Id = z.Id,
                BodyMapId = HumanoidBodyMapId,
                Slug = z.Zone.ToString(),
                Name = z.Name,
                UnitDamage = z.UnitDamage,
            })
            .ToList(),
    };

    public static SlotLayoutDal HumanoidSlotLayout => new()
    {
        Id = HumanoidSlotLayoutId,
        Name = "Гуманоид",
        Entries = new List<SlotLayoutEntryDal>
        {
            new() { SlotLayoutId = HumanoidSlotLayoutId, SlotKey = SlotKeys.Pocket, Count = 3 },
            new() { SlotLayoutId = HumanoidSlotLayoutId, SlotKey = SlotKeys.RightHand, Count = 1 },
            new() { SlotLayoutId = HumanoidSlotLayoutId, SlotKey = SlotKeys.LeftHand, Count = 1 },
        },
    };
}
