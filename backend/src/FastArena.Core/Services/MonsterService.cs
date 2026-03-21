
using FastArena.Core.Domain.Monsters;
using FastArena.Core.Interfaces.App;
using FastArena.Core.Interfaces.Storages;

namespace FastArena.Core.Services;

public class MonsterService : IMonsterService
{
    private readonly IMonsterMoldStorage _monsterMoldStorage;
    private readonly Random _random = new Random();
    private readonly Dictionary<int, string[]> _nameModificatorsMap = new() {
        { -10, new string[] {"Остервене́лый", "Остервене́лая", "Остервене́лое" } },
        { -8, new string[] {"Свирепый", "Свирепая", "Свирепое" } },
        { -6, new string[] {"Жирный", "Жирная", "Жирное" } },
        { -4, new string[] {"Злющий", "Злющая", "Злющее" } },
        { -2, new string[] {"Дерзкий", "Дерзкая", "Дерзкое" } },
        { 0, new string[] {"", "", "" } },
        { 2, new string[] {"", "", "" } },
        { 6, new string[] {"Тощий", "Тощая", "Тощее" } },
        { 8, new string[] {"Хилый", "Хилая", "Хилое" } },
        { 4, new string[] {"Вялый", "Вялая", "Вялое" } },
        { 10, new string[] {"Всратый", "Всратая", "Всратое" } },
        { int.MaxValue, new string[] {"Жалкий", "Жалкая", "Жалкое" } },
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
        var diff = normalizedRank - level;

        var p1 = int.MinValue;
        
        foreach(var modificator in _nameModificatorsMap)
        {
            if (diff > p1 && diff <= modificator.Key)
            {
                return modificator.Value[(int)sex] + ' ' + name;
            }

            p1 = modificator.Key;
        }

        throw new NotImplementedException();
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
