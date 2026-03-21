using FastArena.Core.Domain.Statistic;

namespace FastArena.WebApi.Models;

public class StatisticFilterModel
{
    public Parameter Parameter { get; set; }
    public HeroAliveParam AliveParam { get; set; }
    public HeroOwnerParam OwnerParam { get; set; }
    public bool Desc { get; set; }
}
