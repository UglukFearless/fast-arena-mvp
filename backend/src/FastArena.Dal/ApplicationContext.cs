
using FastArena.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastArena.Dal;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
        //Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserDal>()
            .HasIndex(x => x.Login)
            .IsUnique();

        modelBuilder.Entity<HeroDal>()
            .HasOne(h => h.User)
            .WithMany(u => u.Heroes)
            .HasForeignKey(h => h.UserId);
        

        modelBuilder.Entity<HeroDal>()
            .HasOne(h => h.Portrait)
            .WithMany(p => p.Heroes)
            .HasForeignKey(h => h.PortraitId);

        modelBuilder.Entity<HeroDal>()
            .HasIndex(x => x.Name)
            .IsUnique();

        modelBuilder.Entity<HeroDal>()
            .HasMany(h => h.Items)
            .WithOne(i => i.Hero);

        modelBuilder.Entity<PortraitTagDal>()
            .HasIndex(x => x.Name)
            .IsUnique();

        modelBuilder.Entity<ItemDal>()
            .HasMany(i => i.HeroItems);


        modelBuilder.Entity<ItemDal>()
            .HasMany(i => i.Effects)
            .WithOne(e => e.Item)
            .HasForeignKey(e => e.ItemId);

        modelBuilder.Entity<EffectDefinitionDal>()
            .HasIndex(e => e.ItemId);

        modelBuilder.Entity<EffectDefinitionDal>()
            .HasIndex(e => new { e.ItemId, e.Type });

        modelBuilder.Entity<ItemDal>()
            .HasMany(i => i.OccupiedSlots)
            .WithOne(s => s.Item)
            .HasForeignKey(s => s.ItemId);

        modelBuilder.Entity<ItemSlotDal>()
            .HasIndex(s => s.ItemId);

        modelBuilder.Entity<BodyMapDal>()
            .HasMany(m => m.Zones)
            .WithOne(z => z.BodyMap)
            .HasForeignKey(z => z.BodyMapId);

        modelBuilder.Entity<BodyZoneDal>()
            .HasIndex(z => z.BodyMapId);

        modelBuilder.Entity<SlotLayoutEntryDal>()
            .HasKey(e => new { e.SlotLayoutId, e.SlotKey });

        modelBuilder.Entity<SlotLayoutDal>()
            .HasMany(l => l.Entries)
            .WithOne(e => e.SlotLayout)
            .HasForeignKey(e => e.SlotLayoutId);

        modelBuilder.Entity<HeroItemCellDal>()
            .HasOne(ic => ic.Hero)
            .WithMany(h => h.Items)
            .HasForeignKey(ic => ic.HeroId);

        modelBuilder.Entity<HeroItemCellDal>()
            .HasOne(ic => ic.Item)
            .WithMany(i => i.HeroItems)
            .HasForeignKey(ic => ic.ItemId);

        modelBuilder.Entity<HeroItemCellDal>()
            .HasIndex(ic => ic.HeroId);


        modelBuilder.Entity<HeroSlotDal>()
            .HasOne(s => s.Hero)
            .WithMany(h => h.Slots)
            .HasForeignKey(s => s.HeroId);

        modelBuilder.Entity<HeroSlotDal>()
            .HasIndex(s => s.HeroId);

        modelBuilder.Entity<HeroSlotDal>()
            .HasOne(s => s.HeroItemCell)
            .WithMany()
            .HasForeignKey(s => s.HeroItemCellId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<MonsterMoldDal>()
            .HasOne(mm => mm.Portrait);

        modelBuilder.Entity<MonsterRewardEntryDal>()
            .HasIndex(mre => mre.MonsterMoldId);

        modelBuilder.Entity<MonsterRewardEntryDal>()
            .HasIndex(mre => mre.ItemId);

        modelBuilder.Entity<MonsterRewardEntryDal>()
            .HasOne(mre => mre.MonsterMold)
            .WithMany(mm => mm.RewardEntries)
            .HasForeignKey(mre => mre.MonsterMoldId);

        modelBuilder.Entity<MonsterRewardEntryDal>()
            .HasOne(mre => mre.Item)
            .WithMany(i => i.MonsterRewardEntries)
            .HasForeignKey(mre => mre.ItemId);

        modelBuilder.Entity<MonsterFightResultDal>()
            .HasOne(mfr => mfr.Hero)
            .WithMany(h => h.Results)
            .HasForeignKey(mfr => mfr.HeroId);
        modelBuilder.Entity<MonsterFightResultDal>()
            .HasOne(mfr => mfr.Portrait)
            .WithMany()
            .HasForeignKey(mfr => mfr.MonsterPortraitId);
        modelBuilder.Entity<MonsterFightResultDal>()
            .HasOne(mfr => mfr.MonsterMold)
            .WithMany()
            .HasForeignKey(mfr => mfr.MonsterMoldId);
    }

    public DbSet<UserDal> Users { get; set; }
    public DbSet<HeroDal> Heroes { get; set; }
    public DbSet<PortraitDal> Portraits { get; set; }
    public DbSet<PortraitTagDal> PortraitTags { get; set; }
    public DbSet<ItemDal> Items { get; set; }
    public DbSet<EffectDefinitionDal> EffectDefinitions { get; set; }
    public DbSet<ItemSlotDal> ItemSlots { get; set; }
    public DbSet<BodyMapDal> BodyMaps { get; set; }
    public DbSet<BodyZoneDal> BodyZones { get; set; }
    public DbSet<SlotLayoutDal> SlotLayouts { get; set; }
    public DbSet<SlotLayoutEntryDal> SlotLayoutEntries { get; set; }
    public DbSet<HeroItemCellDal> HeroItemCells { get; set; }
    public DbSet<HeroSlotDal> HeroSlots { get; set; }
    public DbSet<MonsterMoldDal> MonsterMolds { get; set; }
    public DbSet<MonsterRewardEntryDal> MonsterRewardEntries { get; set; }
    public DbSet<MonsterFightResultDal> MonsterFightResults { get; set; }
}
