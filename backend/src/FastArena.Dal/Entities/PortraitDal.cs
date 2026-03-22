
namespace FastArena.Dal.Entities;

public class PortraitDal
{
    public Guid Id { get; set; }
    public required string Url { get; set; }
    public ICollection<PortraitTagDal> Tags { get; set; } = new List<PortraitTagDal>();
    public ICollection<HeroDal>? Heroes { get; set; }
}
