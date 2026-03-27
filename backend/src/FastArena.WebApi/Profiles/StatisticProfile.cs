using FastArena.Core.Domain.Statistic;
using FastArena.WebApi.Dtos;
using FastArena.WebApi.Models;

namespace FastArena.WebApi.Profiles;

internal class StatisticProfile
{
    public static StatisticDataFilter Map(StatisticFilterModel model)
    {
        return new StatisticDataFilter
        {
            Parameter = model.Parameter,
            AliveParam = model.AliveParam,
            OwnerParam = model.OwnerParam,
            Desc = model.Desc,
        };
    }

    public static StatisticDataDto Map(StatisticData model)
    {
        if (model == null)
            return null;

        return new StatisticDataDto
        {
            Title = model.Title,
            ValueTitle = model.ValueTitle,
            ValueSymbols = model.ValueSymbols,
            data = Map(model.Rows.ToList()),
        };
    }

    public static StatisticDataRowDto Map(StatisticDataRow model)
    {
        if (model == null)
            return null;

        return new StatisticDataRowDto
        {
            HeroId = model.HeroId,
            HeroName = model.HeroName,
            PortraitUrl = model.PortraitUrl,
            IsAlive = model.IsAlive,
            Value = model.Value,
        };
    }

    public static List<StatisticDataRowDto> Map(List<StatisticDataRow> models) 
        => models == null ? new List<StatisticDataRowDto>() : models.ConvertAll(Map);
}
