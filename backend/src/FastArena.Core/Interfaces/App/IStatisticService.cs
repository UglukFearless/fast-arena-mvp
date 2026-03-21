using FastArena.Core.Domain.Statistic;

namespace FastArena.Core.Interfaces.App;

public interface IStatisticService
{
    Task<StatisticData> GetAsync(StatisticDataFilter filter, Guid userId);
}
