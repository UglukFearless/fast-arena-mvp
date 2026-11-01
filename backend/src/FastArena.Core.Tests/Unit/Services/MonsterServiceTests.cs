using FastArena.Core.Domain.Monsters;
using FastArena.Core.Interfaces.Storages;
using FastArena.Core.Services;

namespace FastArena.Core.Tests;

public class MonsterServiceTests
{
    private const int RankThreshold = 12;
    private const string MoldName = "Петух";

    [Theory]
    [InlineData(-13, "Остервене́лый")]
    [InlineData(-10, "Остервене́лый")]
    [InlineData(-9, "Свирепый")]
    [InlineData(-8, "Свирепый")]
    [InlineData(-7, "Жирный")]
    [InlineData(-6, "Жирный")]
    [InlineData(-5, "Злющий")]
    [InlineData(-4, "Злющий")]
    [InlineData(-3, "Дерзкий")]
    [InlineData(-2, "Дерзкий")]
    [InlineData(-1, "")]
    [InlineData(0, "")]
    [InlineData(2, "")]
    [InlineData(3, "Вялый")]
    [InlineData(4, "Вялый")]
    [InlineData(5, "Тощий")]
    [InlineData(6, "Тощий")]
    [InlineData(7, "Хилый")]
    [InlineData(8, "Хилый")]
    [InlineData(9, "Всратый")]
    [InlineData(10, "Всратый")]
    [InlineData(11, "Жалкий")]
    public async Task CreateFromMoldAsync_ComposesNameByRankDifference(int difference, string expectedEpithet)
    {
        var service = new MonsterService(new EmptyMonsterMoldStorage());

        var monster = await service.CreateFromMoldAsync(
            RankThreshold - difference,
            CreateMold(MonsterSex.MALE, RankThreshold));

        var expectedName = expectedEpithet.Length == 0
            ? MoldName
            : expectedEpithet + ' ' + MoldName;

        Assert.Equal(expectedName, monster.Name);
    }

    [Fact]
    public async Task CreateFromMoldAsync_RaisesRankBelowThreshold()
    {
        var service = new MonsterService(new EmptyMonsterMoldStorage());

        var monster = await service.CreateFromMoldAsync(1, CreateMold(MonsterSex.MALE, rankLevel: 1));

        Assert.Equal("Жалкий " + MoldName, monster.Name);
    }

    [Theory]
    [InlineData(MonsterSex.MALE, "Вялый")]
    [InlineData(MonsterSex.FEMALE, "Вялая")]
    [InlineData(MonsterSex.NONE, "Вялое")]
    public async Task CreateFromMoldAsync_SelectsEpithetFormBySex(MonsterSex sex, string expectedEpithet)
    {
        var service = new MonsterService(new EmptyMonsterMoldStorage());

        var monster = await service.CreateFromMoldAsync(
            RankThreshold - 3,
            CreateMold(sex, RankThreshold));

        Assert.Equal(expectedEpithet + ' ' + MoldName, monster.Name);
    }

    private static MonsterMold CreateMold(MonsterSex sex, int rankLevel)
    {
        return new MonsterMold
        {
            Id = Guid.NewGuid(),
            Name = MoldName,
            Sex = sex,
            RankLevel = rankLevel,
        };
    }

    private class EmptyMonsterMoldStorage : IMonsterMoldStorage
    {
        public Task<MonsterMold> GetAsync(Guid id)
            => throw new NotSupportedException();

        public Task<ICollection<MonsterMold>> GetAllAsync()
            => throw new NotSupportedException();

        public Task<ICollection<MonsterMold>> GetFiltered(int maxRank)
            => throw new NotSupportedException();
    }
}
