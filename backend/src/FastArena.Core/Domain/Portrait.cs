using FastArena.Core.Domain.Heroes;

namespace FastArena.Core.Domain;

public class Portrait
{
    public Guid Id { get; set; }
    public required string Url { get; set; }
    public ICollection<PortraitTag>? Tags { get; set; }
    public ICollection<Hero>? Heroes { get; set; }
}
