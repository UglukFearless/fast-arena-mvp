using System.ComponentModel.DataAnnotations;

namespace FastArena.Dal.Entities;

public class UserDal
{
    public Guid Id { get; set; }

    [MaxLength(128)]
    public required string Login { get; set; }

    public required string Password { get; set; }

    public Guid? SelectedHeroId { get; set; }

    public ICollection<HeroDal> Heroes { get; set; }
}
