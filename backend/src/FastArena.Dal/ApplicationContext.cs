
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
            .HasMany(i => i.AllowedSlots)
            .WithOne(s => s.Item)
            .HasForeignKey(s => s.ItemId);

        modelBuilder.Entity<ItemAllowedSlotDal>()
            .HasKey(x => new { x.ItemId, x.Slot });

        modelBuilder.Entity<ItemDal>()
            .HasMany(i => i.Effects)
            .WithOne(e => e.Item)
            .HasForeignKey(e => e.ItemId);

        modelBuilder.Entity<EffectDefinitionDal>()
            .HasIndex(e => e.ItemId);

        modelBuilder.Entity<EffectDefinitionDal>()
            .HasIndex(e => new { e.ItemId, e.Type });

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

        modelBuilder.Entity<HeroEquippedSlotDal>()
            .HasKey(es => new { es.HeroId, es.Slot });

        modelBuilder.Entity<HeroEquippedSlotDal>()
            .HasOne(es => es.Hero)
            .WithMany(h => h.EquippedSlots)
            .HasForeignKey(es => es.HeroId);

        modelBuilder.Entity<HeroEquippedSlotDal>()
            .HasOne(es => es.HeroItemCell)
            .WithMany(ic => ic.EquippedSlots)
            .HasForeignKey(es => es.HeroItemCellId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<HeroEquippedSlotDal>()
            .HasIndex(es => es.HeroItemCellId);

        modelBuilder.Entity<MonsterMoldDal>()
            .HasOne(mm => mm.Portrait);

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
    public DbSet<ItemAllowedSlotDal> ItemAllowedSlots { get; set; }
    public DbSet<HeroItemCellDal> HeroItemCells { get; set; }
    public DbSet<HeroEquippedSlotDal> HeroEquippedSlots { get; set; }
    public DbSet<MonsterMoldDal> MonsterMolds { get; set; }
    public DbSet<MonsterFightResultDal> MonsterFightResults { get; set; }
}
