using System.Reflection;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Items;
using FastArena.Core.Services;

namespace FastArena.Core.Tests.Unit.Services.MonsterFight;

public class MonsterFightRewardTests
{
    private static readonly MethodInfo AggregateStackableItemsMethod =
        typeof(MonsterFightService)
            .GetMethod("AggregateStackableItems", BindingFlags.NonPublic | BindingFlags.Static)
        ?? throw new MissingMethodException(nameof(MonsterFightService), "AggregateStackableItems");

    private static List<GivenItem> InvokeAggregate(ICollection<GivenItem> items)
    {
        return (List<GivenItem>) AggregateStackableItemsMethod.Invoke(null, new object[] { items })!;
    }

    private static Item MakeItem(Guid id, bool canBeFolded) => new Item
    {
        Id = id,
        Name = "item",
        Description = string.Empty,
        ItemImage = string.Empty,
        CanBeFolded = canBeFolded,
    };

    [Fact]
    public void AggregateStackableItems_EmptyInput_ReturnsEmpty()
    {
        var result = InvokeAggregate(new List<GivenItem>());

        Assert.Empty(result);
    }

    [Fact]
    public void AggregateStackableItems_SingleStackableItem_ReturnsSameAmount()
    {
        var item = MakeItem(Guid.NewGuid(), canBeFolded: true);

        var result = InvokeAggregate(new List<GivenItem>
        {
            new GivenItem { Item = item, Amount = 3 },
        });

        Assert.Single(result);
        Assert.Equal(3, result[0].Amount);
    }

    [Fact]
    public void AggregateStackableItems_TwoStackableWithSameId_SumsAmounts()
    {
        var item = MakeItem(Guid.NewGuid(), canBeFolded: true);

        var result = InvokeAggregate(new List<GivenItem>
        {
            new GivenItem { Item = item, Amount = 2 },
            new GivenItem { Item = item, Amount = 5 },
        });

        Assert.Single(result);
        Assert.Equal(7, result[0].Amount);
    }

    [Fact]
    public void AggregateStackableItems_TwoStackableWithDifferentIds_BothPreserved()
    {
        var itemA = MakeItem(Guid.NewGuid(), canBeFolded: true);
        var itemB = MakeItem(Guid.NewGuid(), canBeFolded: true);

        var result = InvokeAggregate(new List<GivenItem>
        {
            new GivenItem { Item = itemA, Amount = 1 },
            new GivenItem { Item = itemB, Amount = 4 },
        });

        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.Item.Id == itemA.Id && r.Amount == 1);
        Assert.Contains(result, r => r.Item.Id == itemB.Id && r.Amount == 4);
    }

    [Fact]
    public void AggregateStackableItems_TwoNonStackableWithSameItemId_KeptAsSeparateEntries()
    {
        var item = MakeItem(Guid.NewGuid(), canBeFolded: false);

        var result = InvokeAggregate(new List<GivenItem>
        {
            new GivenItem { Item = item, Amount = 1 },
            new GivenItem { Item = item, Amount = 1 },
        });

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void AggregateStackableItems_MixedItems_StackableAggregatedNonStackableSeparate()
    {
        var stackable = MakeItem(Guid.NewGuid(), canBeFolded: true);
        var nonStackable = MakeItem(Guid.NewGuid(), canBeFolded: false);

        var result = InvokeAggregate(new List<GivenItem>
        {
            new GivenItem { Item = stackable, Amount = 3 },
            new GivenItem { Item = stackable, Amount = 4 },
            new GivenItem { Item = nonStackable, Amount = 1 },
        });

        Assert.Equal(2, result.Count);

        var aggregated = result.Single(r => r.Item.CanBeFolded);
        Assert.Equal(7, aggregated.Amount);

        var notAggregated = result.Single(r => !r.Item.CanBeFolded);
        Assert.Equal(1, notAggregated.Amount);
    }
}
