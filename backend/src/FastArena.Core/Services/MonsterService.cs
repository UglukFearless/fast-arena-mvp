
using FastArena.Core.Domain.Monsters;
using FastArena.Core.Interfaces.App;
using FastArena.Core.Interfaces.Storages;

namespace FastArena.Core.Services;

public class MonsterService : IMonsterService
{
    private readonly IMonsterMoldStorage _monsterMoldStorage;
    private readonly Random _random = new Random();
    private static readonly (int From, int To, string[] Forms)[] _epithetRanges =
    {
        (int.MinValue, -10, new[] { "Остервене́лый", "Остервене́лая", "Остервене́лое" }),
        (-9, -8, new[] { "Свирепый", "Свирепая", "Свирепое" }),
        (-7, -6, new[] { "Жирный", "Жирная", "Жирное" }),
        (-5, -4, new[] { "Злющий", "Злющая", "Злющее" }),
        (-3, -2, new[] { "Дерзкий", "Дерзкая", "Дерзкое" }),
        (-1, 2, new[] { string.Empty, string.Empty, string.Empty }),
        (3, 4, new[] { "Вялый", "Вялая", "Вялое" }),
        (5, 6, new[] { "Тощий", "Тощая", "Тощее" }),
        (7, 8, new[] { "Хилый", "Хилая", "Хилое" }),
        (9, 10, new[] { "Всратый", "Всратая", "Всратое" }),
        (11, int.MaxValue, new[] { "Жалкий", "Жалкая", "Жалкое" }),
    };

    public MonsterService(IMonsterMoldStorage monsterMoldStorage)
    {
        _monsterMoldStorage = monsterMoldStorage;
    }

    public async Task<Monster> CreateFromMoldAsync(int level, MonsterMold mold)
    {
        var monster = new Monster { 
            Id = Guid.NewGuid(),
            MonsterMoldId = mold.Id,
            Mold = mold,
            Level = level,
            Portrait = mold.Portrait,
            MaxHealth = CalcHealht(level, mold.BaseHealth, mold.HealthPerLevel),
            Name = ComposeMonsterName(mold.Name, mold.Sex, level, mold.RankLevel)
        };

        return await Task.FromResult(monster);
    }

    private string ComposeMonsterName(string name, MonsterSex sex, int level, int rankLevel)
    {
        var normalizedRank = rankLevel >= 12 ? rankLevel : 12;
        var difference = normalizedRank - level;
        var epithet = GetEpithetForms(difference)[(int)sex];

        return epithet.Length == 0 ? name : epithet + ' ' + name;
    }

    private static string[] GetEpithetForms(int difference)
    {
        foreach (var range in _epithetRanges)
        {
            if (difference >= range.From && difference <= range.To)
                return range.Forms;
        }

        throw new InvalidOperationException($"No epithet range covers rank difference {difference}.");
    }

    private int CalcHealht(int level, int baseHealth, int healthPerLevel)
    {
        return baseHealth + (level - 1) * healthPerLevel;
    }

    public async Task<ICollection<MonsterMold>> GetAllMoldsAsync()
    {
        return await _monsterMoldStorage.GetAllAsync();
    }

    public async Task<MonsterMold> GetRandomMoldForRankAsync(int rank)
    {
        var molds = await _monsterMoldStorage.GetAllAsync();

        var weightedMolds = molds.Select(mold => new
        {
            Mold = mold,
            Weight = 1.0 / (1 + 0.05 * Math.Pow( Math.Abs(mold.RankLevel - rank), 2))
        }).ToList();

        double totalWeight = weightedMolds.Sum(x => x.Weight);
        double randomWeight = _random.NextDouble() * totalWeight;

        double cumulativeWeight = 0.0;
        foreach (var weightedMonster in weightedMolds)
        {
            cumulativeWeight += weightedMonster.Weight;
            if (cumulativeWeight >= randomWeight)
            {
                return weightedMonster.Mold;
            }
        }

        throw new Exception("There is no any monster mold! O_o!!");
    }
}
