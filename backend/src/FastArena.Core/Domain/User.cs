using FastArena.Core.Domain.Heroes;

namespace FastArena.Core.Domain;

public class User
{
    public Guid Id { get; set; }

    public required string Login { get; set; }

    public required string PasswordHash { get; set; }

    public Guid? SelectedHeroId { get; set; }

    public Hero? SelectedHero { get; set; }

    public ICollection<Hero>? Heroes { get; set; }
}
