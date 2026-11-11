using FastArena.Core.Domain;
using FastArena.Core.Domain.Activities;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Items;
using FastArena.Core.Exceptions;
using FastArena.Core.Interfaces.App;
using FastArena.Core.Interfaces.Storages;
using FastArena.Core.Models;
using FastArena.Core.Services;

namespace FastArena.Core.Tests;

public class HeroEquipmentServiceTests
{

    private static Item CreateWeapon(bool twoHanded = false)
    {
        var slots = twoHanded
            ? new List<ItemSlot>
              {
                  new() { SlotKey = "HAND_RIGHT" },
                  new() { SlotKey = "HAND_LEFT" },
              }
            : new List<ItemSlot>
              {
                  new() { SlotKey = "HAND_RIGHT" },
              };

        return new Item
        {
            Id = Guid.NewGuid(),
            Name = twoHanded ? "Two-Hand Sword" : "One-Hand Sword",
            Description = string.Empty,
            BaseCost = 10,
            ItemImage = "sword.png",
            CanBeFolded = false,
            Type = ItemType.WEAPON,
            OccupiedSlots = slots,
        };
    }

    private static Item CreateShield()
    {
        return new Item
        {
            Id = Guid.NewGuid(),
            Name = "Shield",
            Description = string.Empty,
            BaseCost = 10,
            ItemImage = "shield.png",
            CanBeFolded = false,
            Type = ItemType.SHIELD,
            OccupiedSlots = new List<ItemSlot>
            {
                new() { SlotKey = "HAND_LEFT" },
            },
        };
    }

    private static HeroItemCell WrapInCell(Item item, Guid heroId)
    {
        return new HeroItemCell
        {
            Id = Guid.NewGuid(),
            HeroId = heroId,
            ItemId = item.Id,
            Amount = 1,
            Item = item,
        };
    }

    private static Hero CreateHero(
        Guid userId,
        List<HeroItemCell>? items = null,
        List<HeroSlot>? slots = null)
    {
        var heroId = Guid.NewGuid();
        return new Hero
        {
            Id = heroId,
            Name = "Test Hero",
            Sex = HeroSex.NONE,
            Level = 1,
            Experience = 0,
            MaxHealth = 100,
            UserId = userId,
            IsAlive = HeroAliveState.ALIVE,
            Items = items ?? new List<HeroItemCell>(),
            Slots = slots ?? new List<HeroSlot>
            {
                new() { Id = Guid.NewGuid(), SlotKey = "HAND_RIGHT" },
                new() { Id = Guid.NewGuid(), SlotKey = "HAND_LEFT" },
            },
        };
    }

    private static HeroEquipmentService CreateService(
        Hero hero,
        Guid userId,
        FakeEquipmentStorage? storage = null)
    {
        storage ??= new FakeEquipmentStorage();
        var user = new User
        {
            Id = userId,
            Login = "user",
            PasswordHash = "hash",
            SelectedHeroId = hero.Id,
        };

        return new HeroEquipmentService(
            new FakeUserStorage(user),
            new FakeActivityStateService(isBusy: false),
            new FakeHeroStorage(hero),
            storage);
    }

    [Fact]
    public async Task EquipAsync_ItemForForeignBody_Throws()
    {
        var userId = Guid.NewGuid();
        var ogreArmor = new Item
        {
            Id = Guid.NewGuid(),
            Name = "Ogre Armor",
            Description = string.Empty,
            BaseCost = 10,
            ItemImage = "armor.png",
            CanBeFolded = false,
            Type = ItemType.ARMOR,
            OccupiedSlots = new List<ItemSlot>
            {
                new() { SlotKey = "OGRE_BODY_HEAD" },
            },
        };
        var hero = CreateHero(userId);
        var cell = WrapInCell(ogreArmor, hero.Id);
        hero.Items!.Add(cell);

        var storage = new FakeEquipmentStorage();
        var sut = CreateService(hero, userId, storage);

        await Assert.ThrowsAsync<ActionDeniedException>(() => sut.EquipAsync(userId, cell.Id));
        Assert.False(storage.EquipCalled);
    }

    [Fact]
    public async Task EquipAsync_TwoHandedWeapon_SecondHandOccupied_Throws()
    {
        var userId = Guid.NewGuid();
        var heroId = Guid.NewGuid();
        var sword = CreateWeapon(twoHanded: true);
        var shield = CreateShield();
        var shieldCell = WrapInCell(shield, heroId);

        var hero = new Hero
        {
            Id = heroId,
            Name = "Test Hero",
            Sex = HeroSex.NONE,
            Level = 1,
            Experience = 0,
            MaxHealth = 100,
            UserId = userId,
            IsAlive = HeroAliveState.ALIVE,
            Items = new List<HeroItemCell> { shieldCell },
            Slots = new List<HeroSlot>
            {
                new() { Id = Guid.NewGuid(), SlotKey = "HAND_RIGHT" },
                new() { Id = Guid.NewGuid(), SlotKey = "HAND_LEFT", HeroItemCellId = shieldCell.Id, HeroItemCell = shieldCell },
            },
        };
        var cell = WrapInCell(sword, heroId);
        hero.Items.Add(cell);

        var storage = new FakeEquipmentStorage();
        var sut = CreateService(hero, userId, storage);

        await Assert.ThrowsAsync<ActionDeniedException>(() => sut.EquipAsync(userId, cell.Id));
        Assert.False(storage.EquipCalled);
    }

    [Fact]
    public async Task EquipAsync_OneHandedWeapon_EquipsToRightHand()
    {
        var userId = Guid.NewGuid();
        var weapon = CreateWeapon(twoHanded: false);
        var hero = CreateHero(userId);
        var cell = WrapInCell(weapon, hero.Id);
        hero.Items!.Add(cell);

        var storage = new FakeEquipmentStorage();
        var sut = CreateService(hero, userId, storage);

        await sut.EquipAsync(userId, cell.Id);

        Assert.True(storage.EquipCalled);
        var occupiedSlotId = Assert.Single(storage.LastEquippedSlotIds);
        Assert.Equal("HAND_RIGHT", hero.Slots!.Single(s => s.Id == occupiedSlotId).SlotKey);
    }

    [Fact]
    public async Task EquipAsync_ItemWithRepeatedKey_OccupiesTwoSlotsOfThatKey()
    {
        var userId = Guid.NewGuid();
        var amulet = new Item
        {
            Id = Guid.NewGuid(),
            Name = "Big Amulet",
            Description = string.Empty,
            BaseCost = 10,
            ItemImage = "amulet.png",
            CanBeFolded = false,
            Type = ItemType.OTHER,
            OccupiedSlots = new List<ItemSlot>
            {
                new() { SlotKey = "POCKET" },
                new() { SlotKey = "POCKET" },
            },
        };

        var heroId = Guid.NewGuid();
        var pockets = new List<HeroSlot>
        {
            new() { Id = Guid.NewGuid(), SlotKey = "POCKET" },
            new() { Id = Guid.NewGuid(), SlotKey = "POCKET" },
            new() { Id = Guid.NewGuid(), SlotKey = "POCKET" },
        };

        var hero = new Hero
        {
            Id = heroId,
            Name = "Test Hero",
            Sex = HeroSex.NONE,
            Level = 1,
            Experience = 0,
            MaxHealth = 100,
            UserId = userId,
            IsAlive = HeroAliveState.ALIVE,
            Items = new List<HeroItemCell>(),
            Slots = pockets,
        };
        var cell = WrapInCell(amulet, heroId);
        hero.Items.Add(cell);

        var storage = new FakeEquipmentStorage();
        var sut = CreateService(hero, userId, storage);

        await sut.EquipAsync(userId, cell.Id);

        Assert.Equal(2, storage.LastEquippedSlotIds.Count);
        Assert.All(storage.LastEquippedSlotIds, id => Assert.Equal("POCKET", pockets.Single(p => p.Id == id).SlotKey));
    }

    [Fact]
    public async Task EquipAsync_ItemWithRepeatedKey_NotEnoughFreeSlots_Throws()
    {
        var userId = Guid.NewGuid();
        var amulet = new Item
        {
            Id = Guid.NewGuid(),
            Name = "Big Amulet",
            Description = string.Empty,
            BaseCost = 10,
            ItemImage = "amulet.png",
            CanBeFolded = false,
            Type = ItemType.OTHER,
            OccupiedSlots = new List<ItemSlot>
            {
                new() { SlotKey = "POCKET" },
                new() { SlotKey = "POCKET" },
            },
        };

        var hero = CreateHero(userId, slots: new List<HeroSlot>
        {
            new() { Id = Guid.NewGuid(), SlotKey = "POCKET" },
        });
        var cell = WrapInCell(amulet, hero.Id);
        hero.Items!.Add(cell);

        var storage = new FakeEquipmentStorage();
        var sut = CreateService(hero, userId, storage);

        await Assert.ThrowsAsync<ActionDeniedException>(() => sut.EquipAsync(userId, cell.Id));
        Assert.False(storage.EquipCalled);
    }

    [Fact]
    public async Task EquipAsync_TwoHandedWeapon_HandsFree_Succeeds()
    {
        var userId = Guid.NewGuid();
        var weapon = CreateWeapon(twoHanded: true);
        var hero = CreateHero(userId);
        var cell = WrapInCell(weapon, hero.Id);
        hero.Items!.Add(cell);

        var storage = new FakeEquipmentStorage();
        var sut = CreateService(hero, userId, storage);

        await sut.EquipAsync(userId, cell.Id);

        Assert.True(storage.EquipCalled);
    }

    [Fact]
    public async Task EquipAsync_TwoHandedWeapon_HandOccupied_Throws()
    {
        var userId = Guid.NewGuid();
        var twoHander = CreateWeapon(twoHanded: true);
        var oneHander = CreateWeapon(twoHanded: false);

        var heroId = Guid.NewGuid();
        var existingCell = WrapInCell(oneHander, heroId);

        var hero = new Hero
        {
            Id = heroId,
            Name = "Test Hero",
            Sex = HeroSex.NONE,
            Level = 1,
            Experience = 0,
            MaxHealth = 100,
            UserId = userId,
            IsAlive = HeroAliveState.ALIVE,
            Items = new List<HeroItemCell> { existingCell },
            Slots = new List<HeroSlot>
            {
                new()
                {
                    Id = Guid.NewGuid(),
            HeroId = heroId,
                    SlotKey = "HAND_RIGHT",
                    HeroItemCellId = existingCell.Id,
                    HeroItemCell = existingCell,
                },
            },
        };

        var newCell = WrapInCell(twoHander, heroId);
        hero.Items.Add(newCell);

        var sut = CreateService(hero, userId);

        await Assert.ThrowsAsync<ActionDeniedException>(() => sut.EquipAsync(userId, newCell.Id));
    }

    [Fact]
    public async Task EquipAsync_ShieldWhileTwoHandedEquipped_Throws()
    {
        var userId = Guid.NewGuid();
        var twoHander = CreateWeapon(twoHanded: true);
        var shield = CreateShield();

        var heroId = Guid.NewGuid();
        var twoHanderCell = WrapInCell(twoHander, heroId);

        var hero = new Hero
        {
            Id = heroId,
            Name = "Test Hero",
            Sex = HeroSex.NONE,
            Level = 1,
            Experience = 0,
            MaxHealth = 100,
            UserId = userId,
            IsAlive = HeroAliveState.ALIVE,
            Items = new List<HeroItemCell> { twoHanderCell },
            Slots = new List<HeroSlot>
            {
                new()
                {
                    Id = Guid.NewGuid(),
            HeroId = heroId,
                    SlotKey = "HAND_RIGHT",
                    HeroItemCellId = twoHanderCell.Id,
                    HeroItemCell = twoHanderCell,
                },
                new()
                {
                    Id = Guid.NewGuid(),
            HeroId = heroId,
                    SlotKey = "HAND_LEFT",
                    HeroItemCellId = twoHanderCell.Id,
                    HeroItemCell = twoHanderCell,
                },
            },
        };

        var shieldCell = WrapInCell(shield, heroId);
        hero.Items.Add(shieldCell);

        var sut = CreateService(hero, userId);

        await Assert.ThrowsAsync<ActionDeniedException>(() => sut.EquipAsync(userId, shieldCell.Id));
    }

    [Fact]
    public async Task EquipAsync_AllSlotsOccupied_Throws()
    {
        var userId = Guid.NewGuid();
        var weapon1 = CreateWeapon(twoHanded: false);
        var weapon2 = CreateWeapon(twoHanded: false);

        var heroId = Guid.NewGuid();
        var cell1 = WrapInCell(weapon1, heroId);
        var cell2 = WrapInCell(weapon2, heroId);

        var hero = new Hero
        {
            Id = heroId,
            Name = "Test Hero",
            Sex = HeroSex.NONE,
            Level = 1,
            Experience = 0,
            MaxHealth = 100,
            UserId = userId,
            IsAlive = HeroAliveState.ALIVE,
            Items = new List<HeroItemCell> { cell1, cell2 },
            Slots = new List<HeroSlot>
            {
                new()
                {
                    Id = Guid.NewGuid(),
            HeroId = heroId,
                    SlotKey = "HAND_RIGHT",
                    HeroItemCellId = cell1.Id,
                    HeroItemCell = cell1,
                },
            },
        };

        var sut = CreateService(hero, userId);

        // weapon2's only allowed slot (RIGHT_HAND) is occupied
        await Assert.ThrowsAsync<ActionDeniedException>(() => sut.EquipAsync(userId, cell2.Id));
    }

    [Fact]
    public async Task EquipAsync_ItemNotOwned_Throws()
    {
        var userId = Guid.NewGuid();
        var hero = CreateHero(userId);
        var sut = CreateService(hero, userId);

        await Assert.ThrowsAsync<ActionDeniedException>(() => sut.EquipAsync(userId, Guid.NewGuid()));
    }

    [Fact]
    public async Task UnequipAsync_OccupiedHandSlot_Succeeds()
    {
        var userId = Guid.NewGuid();
        var weapon = CreateWeapon(twoHanded: false);

        var heroId = Guid.NewGuid();
        var cell = WrapInCell(weapon, heroId);

        var hero = new Hero
        {
            Id = heroId,
            Name = "Test Hero",
            Sex = HeroSex.NONE,
            Level = 1,
            Experience = 0,
            MaxHealth = 100,
            UserId = userId,
            IsAlive = HeroAliveState.ALIVE,
            Items = new List<HeroItemCell> { cell },
            Slots = new List<HeroSlot>
            {
                new()
                {
                    Id = Guid.NewGuid(),
            HeroId = heroId,
                    SlotKey = "HAND_RIGHT",
                    HeroItemCellId = cell.Id,
                    HeroItemCell = cell,
                },
            },
        };

        var storage = new FakeEquipmentStorage();
        var sut = CreateService(hero, userId, storage);

        await sut.UnequipAsync(userId, cell.Id);

        Assert.True(storage.UnequipCalled);
    }

    [Fact]
    public async Task UnequipAsync_EmptySlot_Throws()
    {
        var userId = Guid.NewGuid();
        var hero = CreateHero(userId);
        var sut = CreateService(hero, userId);

        await Assert.ThrowsAsync<ActionDeniedException>(
            () => sut.UnequipAsync(userId, Guid.NewGuid()));
    }

    private sealed class FakeUserStorage : IUserStorage
    {
        private readonly User _user;
        public FakeUserStorage(User user) => _user = user;

        public Task<User> GetAsync(Guid id) => Task.FromResult(_user);
        public Task<User> CreateAsync(UserCreationModel model) => throw new NotImplementedException();
        public Task<User> GetByLoginAsync(string login) => throw new NotImplementedException();
        public Task SelectHeroAsync(Guid id, Guid heroId) => throw new NotImplementedException();
        public Task UnselectHeroAsync(Guid id) => throw new NotImplementedException();
    }

    private sealed class FakeActivityStateService : IActivityStateService
    {
        private readonly bool _isBusy;
        public FakeActivityStateService(bool isBusy) => _isBusy = isBusy;

        public Task<bool> IsBusyAsync(Guid userId) => Task.FromResult(_isBusy);
        public Task CreateStateAsync(ActivitySession session) => throw new NotImplementedException();
        public Task UpdateStateAsync(ActivitySession session) => throw new NotImplementedException();
        public Task<ActivitySession?> GetByHeroIdAsync(Guid heroId) => throw new NotImplementedException();
        public Task<ActivitySession?> GetAsync(Guid id) => throw new NotImplementedException();
        public Task<SemaphoreSlim> GetLockerForSessionIdAsync(Guid sessionId) => throw new NotImplementedException();
        public Task CleanTheStateAsync(Guid id) => throw new NotImplementedException();
        public Task CleanTheLocker(Guid id) => throw new NotImplementedException();
    }

    private sealed class FakeHeroStorage : IHeroStorage
    {
        private readonly Hero _hero;
        public FakeHeroStorage(Hero hero) => _hero = hero;

        public Task<Hero> GetAsync(Guid id) => Task.FromResult(_hero);
        public Task<ICollection<Hero>> GetAllByUserIdAsync(Guid userId) => throw new NotImplementedException();
        public Task<ICollection<Hero>> GetAllWithIncludesAsync() => throw new NotImplementedException();
        public Task<ICollection<Hero>> GetAllWithIncludesByUserIdAsync(Guid userId) => throw new NotImplementedException();
        public Task<Hero> CreateAsync(HeroCreationModel model) => throw new NotImplementedException();
        public Task<List<HeroItemCell>> GiveItemsToHeroAsync(Guid heroId, ICollection<GivenItem> items) => throw new NotImplementedException();
        public Task ExchangeHeroItemsAsync(Guid heroId, ICollection<HeroItemTakeRequest> itemsToTake, ICollection<GivenItem> itemsToGive, int moneyToTake, int moneyToGive) => throw new NotImplementedException();
        public Task<Hero> UpdateHeroAsync(Hero hero) => throw new NotImplementedException();
    }

    private sealed class FakeEquipmentStorage : IHeroEquipmentStorage
    {
        public bool EquipCalled { get; private set; }
        public bool UnequipCalled { get; private set; }
        public IReadOnlyList<Guid> LastEquippedSlotIds { get; private set; } = Array.Empty<Guid>();

        public Task EquipItemAsync(Guid heroId, Guid heroItemCellId, IReadOnlyList<Guid> slotIds)
        {
            EquipCalled = true;
            LastEquippedSlotIds = slotIds;
            return Task.CompletedTask;
        }

        public Task UnequipItemAsync(Guid heroId, Guid heroItemCellId)
        {
            UnequipCalled = true;
            return Task.CompletedTask;
        }

        public Task<HeroItemCell> ConsumePocketItemAsync(Guid heroId, Guid heroItemCellId) =>
            throw new NotImplementedException();
    }
}