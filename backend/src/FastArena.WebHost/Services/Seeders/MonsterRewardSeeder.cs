using FastArena.Dal;
using FastArena.Dal.Entities;
using FastArena.WebHost.Services.Seeders.Data;
using Microsoft.EntityFrameworkCore;

namespace FastArena.WebHost.Services.Seeders;

public class MonsterRewardSeeder
{
    private readonly ApplicationContext _context;

    public MonsterRewardSeeder(ApplicationContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        var seededMoldIds = (await _context.MonsterRewardEntries
            .Select(e => e.MonsterMoldId)
            .Distinct()
            .ToListAsync())
            .ToHashSet();

        var seedRows = new List<(Guid MonsterMoldId, Guid ItemId, int ChancePercent, int Amount)>
        {
            (MonsterSeedData.GnomId,       ItemSeedData.HairBunchId,      90, 1),

            (MonsterSeedData.BullyId,      ItemSeedData.ToothId,          60, 1),
            (MonsterSeedData.BullyId,      ItemSeedData.ToothId,          60, 1),
            (MonsterSeedData.BullyId,      ItemSeedData.ToothId,          60, 1),
            (MonsterSeedData.BullyId,      ItemSeedData.ToothId,          60, 1),
            (MonsterSeedData.BullyId,      ItemSeedData.EyeId,            30, 1),

            (MonsterSeedData.DevilId,      ItemSeedData.HornId,           60, 1),
            (MonsterSeedData.DevilId,      ItemSeedData.HornId,           60, 1),

            (MonsterSeedData.FairyId,      ItemSeedData.PixieDustId,      90, 1),

            (MonsterSeedData.SlugId,       ItemSeedData.EyeId,            60, 1),
            (MonsterSeedData.SlugId,       ItemSeedData.EyeId,            60, 1),

            (MonsterSeedData.GolemId,      ItemSeedData.IceHeartId,      100, 1),

            (MonsterSeedData.MemberbotId,  ItemSeedData.GearId,           80, 1),

            (MonsterSeedData.KaloidId,     ItemSeedData.StoneHeartId,    100, 1),

            (MonsterSeedData.RoosterId,    ItemSeedData.RoosterFeatherId, 50, 1),
            (MonsterSeedData.RoosterId,    ItemSeedData.RoosterFeatherId, 50, 1),
            (MonsterSeedData.RoosterId,    ItemSeedData.RoosterFeatherId, 50, 1),
            (MonsterSeedData.RoosterId,    ItemSeedData.RoosterFeatherId, 50, 1),

            (MonsterSeedData.ZombieId,     ItemSeedData.ToothId,          50, 1),
            (MonsterSeedData.ZombieId,     ItemSeedData.ToothId,          50, 1),
            (MonsterSeedData.ZombieId,     ItemSeedData.ToothId,          50, 1),
            (MonsterSeedData.ZombieId,     ItemSeedData.ToothId,          50, 1),

            (MonsterSeedData.GhostId,      ItemSeedData.EctoplasmId,      70, 1),

            (MonsterSeedData.DragonId,     ItemSeedData.DragonHeartId,   100, 1),
            (MonsterSeedData.DragonId,     ItemSeedData.GemId,           100, 1),

            (MonsterSeedData.TrogloditeId, ItemSeedData.ClawId,           70, 1),
            (MonsterSeedData.TrogloditeId, ItemSeedData.ClawId,           70, 1),

            (MonsterSeedData.WerewolfId,   ItemSeedData.FangId,           80, 1),
            (MonsterSeedData.WerewolfId,   ItemSeedData.FangId,           60, 1),
        };

        var entriesToAdd = new List<MonsterRewardEntryDal>();

        foreach (var row in seedRows)
        {
            // Skip the whole mold if it already has any reward entries.
            if (seededMoldIds.Contains(row.MonsterMoldId))
            {
                continue;
            }

            entriesToAdd.Add(new MonsterRewardEntryDal
            {
                Id = Guid.NewGuid(),
                MonsterMoldId = row.MonsterMoldId,
                ItemId = row.ItemId,
                ChancePercent = row.ChancePercent,
                Amount = row.Amount,
            });
        }

        if (entriesToAdd.Count == 0)
        {
            return;
        }

        _context.MonsterRewardEntries.AddRange(entriesToAdd);
        await _context.SaveChangesAsync();
    }
}
