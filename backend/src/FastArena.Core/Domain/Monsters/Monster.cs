namespace FastArena.Core.Domain.Monsters;

public class Monster
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int MaxHealth { get; set; }
    public int Level { get; set; }
    public Guid MonsterMoldId { get; set; }
    public MonsterMold Mold { get; set; }
    public Portrait Portrait { get; set; }
}
