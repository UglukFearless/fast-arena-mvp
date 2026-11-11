using FastArena.Core.Domain;
using FastArena.Core.Domain.Bodies;
using FastArena.Core.Domain.Heroes;

namespace FastArena.Core.Tests;

public class HeroSlotsTests
{
    private static BodyMap CreateHumanoidBodyMap()
    {
        var bodyMapId = Guid.NewGuid();
        return new BodyMap
        {
            Id = bodyMapId,
            Slug = "HUMANOID",
            Name = "Humanoid",
            Zones = new List<BodyZone>
            {
                new() { Id = Guid.NewGuid(), BodyMapId = bodyMapId, Slug = "HEAD", Name = "Голова", UnitDamage = 20 },
                new() { Id = Guid.NewGuid(), BodyMapId = bodyMapId, Slug = "HEART", Name = "Сердце", UnitDamage = 30 },
            },
        };
    }

    private static SlotLayout CreateHumanoidLayout()
    {
        var layoutId = Guid.NewGuid();
        return new SlotLayout
        {
            Id = layoutId,
            Name = "Humanoid",
            Entries = new List<SlotLayoutEntry>
            {
                new() { SlotLayoutId = layoutId, SlotKey = "POCKET", Count = 3 },
                new() { SlotLayoutId = layoutId, SlotKey = "HAND_RIGHT", Count = 1 },
                new() { SlotLayoutId = layoutId, SlotKey = "HAND_LEFT", Count = 1 },
            },
        };
    }

    [Fact]
    public void Create_ZoneSlotsCarryBodyMapSlug()
    {
        var heroId = Guid.NewGuid();

        var slots = HeroSlots.Create(heroId, CreateHumanoidBodyMap(), CreateHumanoidLayout());

        Assert.Contains(slots, s => s.SlotKey == "HUMANOID_BODY_HEAD");
        Assert.Contains(slots, s => s.SlotKey == "HUMANOID_BODY_HEART");
        Assert.All(slots, s => Assert.Equal(heroId, s.HeroId));
    }

    [Fact]
    public void Create_HandsAndPocketsFollowLayoutWithoutBodyPrefix()
    {
        var slots = HeroSlots.Create(Guid.NewGuid(), CreateHumanoidBodyMap(), CreateHumanoidLayout());

        Assert.Equal(3, slots.Count(s => s.SlotKey == "POCKET"));
        Assert.Single(slots, s => s.SlotKey == "HAND_RIGHT");
        Assert.Single(slots, s => s.SlotKey == "HAND_LEFT");
        Assert.DoesNotContain(slots, s => s.SlotKey.Contains("BODY", StringComparison.Ordinal) && !s.SlotKey.StartsWith("HUMANOID_BODY_", StringComparison.Ordinal));
    }

    [Fact]
    public void Zone_KeyDiffersBetweenBodies()
    {
        var humanoid = CreateHumanoidBodyMap();
        var ogre = new BodyMap
        {
            Id = Guid.NewGuid(),
            Slug = "OGRE",
            Name = "Ogre",
            Zones = new List<BodyZone>
            {
                new() { Id = Guid.NewGuid(), BodyMapId = Guid.NewGuid(), Slug = "HEAD", Name = "Голова", UnitDamage = 20 },
            },
        };

        Assert.NotEqual(
            SlotKeys.Zone(humanoid, humanoid.Zones![0]),
            SlotKeys.Zone(ogre, ogre.Zones![0]));
    }
}
