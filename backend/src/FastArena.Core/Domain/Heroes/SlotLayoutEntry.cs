namespace FastArena.Core.Domain.Heroes;

public class SlotLayoutEntry
{
    public Guid SlotLayoutId { get; set; }
    public required string SlotKey { get; set; }
    public int Count { get; set; }

    public SlotLayout? SlotLayout { get; set; }
}
