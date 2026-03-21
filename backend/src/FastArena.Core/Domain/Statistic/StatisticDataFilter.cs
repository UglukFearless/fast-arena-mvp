namespace FastArena.Core.Domain.Statistic;

public class StatisticDataFilter
{
    public Parameter Parameter { get; set; }
    public HeroAliveParam AliveParam { get; set; }
    public HeroOwnerParam OwnerParam { get; set; }
    public bool Desc { get; set; }
}
