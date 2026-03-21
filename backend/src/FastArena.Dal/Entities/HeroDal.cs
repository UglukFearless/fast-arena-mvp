using System.ComponentModel.DataAnnotations;
using FastArena.Core.Domain.Heroes;

namespace FastArena.Dal.Entities;

public class HeroDal
{
    public Guid Id { get; set; }

    [MaxLength(256)]
    public required string Name { get; set; }

    public required HeroSex Sex { get; set; }

    public required int Level { get; set; }

    public required long Experience { get; set; }

    public Guid PortraitId { get; set; }

    public required HeroAliveState IsAlive { get; set; }

    public required int MaxHealth { get; set; }

    public required Guid UserId { get; set; }

    public UserDal? User { get; set; }

    public PortraitDal? Portrait { get; set; }

    public ICollection<HeroItemCellDal> Items { get; set; } = new List<HeroItemCellDal>();
    public ICollection<MonsterFightResultDal> Results { get; set; } = new List<MonsterFightResultDal>();
}
