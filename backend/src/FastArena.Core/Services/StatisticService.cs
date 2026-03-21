using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Items;
using FastArena.Core.Domain.MondterFights;
using FastArena.Core.Domain.Statistic;
using FastArena.Core.Interfaces.App;

namespace FastArena.Core.Services;

public class StatisticService : IStatisticService
{
    private readonly IHeroService _heroService;
    public StatisticService(IHeroService heroService)
    {
        _heroService = heroService;
    }
    public async Task<StatisticData> GetAsync(StatisticDataFilter filter, Guid userId)
    {
        
        var statistic = new StatisticData
        {
            Title = BuildTitle(filter),
            ValueTitle = BuildValueTitle(filter.Parameter),
            ValueSymbols = GetValueSymbols(filter.Parameter),
            Rows = await GetStatisticRows(filter, userId)
        };
        return statistic;
    }

    private string BuildTitle(StatisticDataFilter filter) => filter.Parameter switch
    {
        Parameter.WINS => "Количество побед",
        Parameter.GOLD => "Самый богатый",
        Parameter.LEVEL => "Самый опытный",
        _ => throw new NotImplementedException(),
    };

    private string BuildValueTitle(Parameter param) => param switch
    {
        Parameter.WINS => "Победы",
        Parameter.GOLD => "Золото",
        Parameter.LEVEL => "Уровень",
        _ => throw new NotImplementedException(),
    };

    private string GetValueSymbols(Parameter param) => param switch
    {
        Parameter.WINS => "w",
        Parameter.GOLD => "g",
        Parameter.LEVEL => "lvl",
        _ => throw new NotImplementedException(),
    };

    private async Task<ICollection<StatisticDataRow>> GetStatisticRows(StatisticDataFilter filter, Guid userId)
    {
        var heroes = (await GetHeroesAsync(filter.OwnerParam, userId));

        if (filter.AliveParam == HeroAliveParam.ALIVE)
        {
            heroes = heroes.Where(h => h.IsAlive == HeroAliveState.ALIVE).ToList();
        } 
        else if (filter.AliveParam == HeroAliveParam.DEAD)
        {
            heroes = heroes.Where(h => h.IsAlive == HeroAliveState.DEAD).ToList();
        }

        var rows = heroes.ConvertAll(h => ConvertHeroToRow(h, filter.Parameter));

        if (filter.Desc)
        {
            rows = rows.OrderByDescending(r => r.Value).ToList();
        } 
        else
        {
            rows = rows.OrderBy(r => r.Value).ToList();
        }

        return rows;
    }

    private StatisticDataRow ConvertHeroToRow(Hero h, Parameter parameter)
    {
        return new StatisticDataRow
        {
            HeroName = h.Name,
            IsAlive = h.IsAlive == HeroAliveState.ALIVE,
            PortraitUrl = h.Portrait.Url,
            Value = GetValueForHero(h, parameter),
        };
    }

    private int GetValueForHero(Hero h, Parameter parameter)
    {
        switch (parameter)
        {
            case Parameter.WINS:
                return h.Results.Where(r => r.Type == MonsterFightResultType.VICTORY).Count();
            case Parameter.GOLD:
                var goldItem = h.Items?.FirstOrDefault(ic => ic.Item?.Type == ItemType.MONEY);
                return goldItem == null ? 0 : goldItem.Amount;
            case Parameter.LEVEL:
                return h.Level;
            default:
                throw new Exception("Unknown statistic main parameter! " + parameter);
        }
    }

    private async Task<List<Hero>> GetHeroesAsync(HeroOwnerParam owner, Guid userId)
    {
        List<Hero> heroes;
        if (owner == HeroOwnerParam.MINE)
        {
            heroes = (await _heroService.GetAllByUserIdAsync(userId)).ToList();
        }
        else
        {
            heroes = (await _heroService.GetAllWithInfoAsync()).ToList();
        }
        return heroes;
    }
}
