
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

        new DbInitializer().Seed();
    }

    public DbSet<UserDal> Users { get; set; }
    public DbSet<HeroDal> Heroes { get; set; }
    public DbSet<PortraitDal> Portraits { get; set; }
    public DbSet<PortraitTagDal> PortraitTags { get; set; }
    public DbSet<ItemDal> Items { get; set; }
    public DbSet<HeroItemCellDal> HeroItemCells { get; set; }
    public DbSet<MonsterMoldDal> MonsterMolds { get; set; }
    public DbSet<MonsterFightResultDal> MonsterFightResults { get; set; }
}
