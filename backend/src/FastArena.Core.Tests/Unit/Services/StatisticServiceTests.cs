using FastArena.Core.Domain;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.MondterFights;
using FastArena.Core.Domain.Statistic;
using FastArena.Core.Interfaces.App;
using FastArena.Core.Models;
using FastArena.Core.Services;

namespace FastArena.Core.Tests;

public class StatisticServiceTests
{
    [Fact]
    public async Task GetAsync_ExperienceParameter_UsesAccumulatedExperience()
    {
        var hero = CreateHero(level: 5, experience: 1234);
        var service = CreateService(hero);

        var data = await service.GetAsync(CreateFilter(Parameter.EXPERIENCE), Guid.NewGuid());

        var row = Assert.Single(data.Rows);
        Assert.Equal(1234, row.Value);
        Assert.Equal("Опыт", data.ValueTitle);
        Assert.Equal("xp", data.ValueSymbols);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GetAsync_EqualValues_PlacesAliveHeroAboveDead(bool desc)
    {
        var dead = CreateHero(level: 3, experience: 500, isAlive: false, name: "Dead");
        var alive = CreateHero(level: 3, experience: 500, isAlive: true, name: "Alive");
        var service = CreateService(dead, alive);

        var data = await service.GetAsync(CreateFilter(Parameter.EXPERIENCE, desc), Guid.NewGuid());

        Assert.Equal(new[] { "Alive", "Dead" }, data.Rows.Select(r => r.HeroName));
    }

    private static StatisticService CreateService(params Hero[] heroes)
    {
        return new StatisticService(new FakeHeroService(heroes));
    }

    private static StatisticDataFilter CreateFilter(Parameter parameter, bool desc = true)
    {
        return new StatisticDataFilter
        {
            Parameter = parameter,
            AliveParam = HeroAliveParam.ALL,
            OwnerParam = HeroOwnerParam.ANY,
            Desc = desc,
        };
    }

    private static Hero CreateHero(
        int level,
        long experience,
        bool isAlive = true,
        string name = "Hero")
    {
        return new Hero
        {
            Id = Guid.NewGuid(),
            Name = name,
            Sex = HeroSex.NONE,
            Level = level,
            Experience = experience,
            IsAlive = isAlive ? HeroAliveState.ALIVE : HeroAliveState.DEAD,
            Portrait = new Portrait { Id = Guid.NewGuid(), Url = "/portraits/hero.png" },
            Results = new List<MonsterFightResult>(),
            Items = new List<HeroItemCell>(),
        };
    }

    private sealed class FakeHeroService : IHeroService
    {
        private readonly ICollection<Hero> _heroes;

        public FakeHeroService(ICollection<Hero> heroes)
        {
            _heroes = heroes;
        }

        public Task<ICollection<Hero>> GetAllByUserIdAsync(Guid userId)
            => throw new NotImplementedException();

        public Task<ICollection<Hero>> GetAllWithInfoByUserIdAsync(Guid userId)
            => Task.FromResult(_heroes);

        public Task<ICollection<Hero>> GetAllWithInfoAsync()
            => Task.FromResult(_heroes);

        public Task<Hero> GetSelectedByUserIdAsync(Guid userId)
            => throw new NotImplementedException();

        public Task<Hero> CreateAsync(HeroCreationModel model)
            => throw new NotImplementedException();

        public Task SelectForUserAsync(Guid id, Guid userId)
            => throw new NotImplementedException();

        public Task UnselectForUserAsync(Guid userId)
            => throw new NotImplementedException();

        public Task UnselectForUserForceAsync(Guid userId)
            => throw new NotImplementedException();

        public Task<Hero> GetAsync(Guid id)
            => throw new NotImplementedException();

        public Task KillTheHeroAsync(Guid id)
            => throw new NotImplementedException();

        public Task GiveItemsToHeroAsync(Guid heroId, ICollection<GivenItem> items)
            => throw new NotImplementedException();

        public Task<HeroItemCell> ConsumePocketItemForFightAsync(Guid heroId, Guid heroItemCellId)
            => throw new NotImplementedException();

        public Task IncreaseExperienceAsync(int experience, Guid heroId)
            => throw new NotImplementedException();
    }
}
