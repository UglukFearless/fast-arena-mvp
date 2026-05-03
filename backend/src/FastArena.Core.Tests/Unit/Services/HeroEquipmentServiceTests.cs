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
    // ── helpers ──────────────────────────────────────────────────────────────

    private static Item CreateWeapon(bool twoHanded = false)
    {
        var slots = twoHanded
            ? new List<ItemAllowedSlot>
              {
                  new() { Slot = EquipmentSlotType.RIGHT_HAND },
                  new() { Slot = EquipmentSlotType.LEFT_HAND },
              }
            : new List<ItemAllowedSlot>
              {
                  new() { Slot = EquipmentSlotType.RIGHT_HAND },
              };

        return new Item
        {
            Id = Guid.NewGuid(),
            Name = twoHanded ? "Two-Hand Sword" : "One-Hand Sword",
            Description = string.Empty,
            BaseCost = 10,
            ItemImage = "sword.png",
            CanBeEquipped = true,
            CanBeFolded = false,
            Type = ItemType.WEAPON,
            AllowedSlots = slots,
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
            CanBeEquipped = true,
            CanBeFolded = false,
            Type = ItemType.SHIELD,
            AllowedSlots = new List<ItemAllowedSlot>
            {
                new() { Slot = EquipmentSlotType.LEFT_HAND },
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
        List<HeroEquippedSlot>? equippedSlots = null)
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
            EquippedSlots = equippedSlots ?? new List<HeroEquippedSlot>(),
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

    // ── equip: one-handed weapon ──────────────────────────────────────────────

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
        Assert.Equal(EquipmentSlotType.RIGHT_HAND, storage.LastEquippedSlot);
    }

    // ── equip: two-handed weapon ──────────────────────────────────────────────

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
            EquippedSlots = new List<HeroEquippedSlot>
            {
                new()
                {
                    HeroId = heroId,
                    Slot = EquipmentSlotType.RIGHT_HAND,
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

    // ── equip: shield while two-handed weapon is equipped ────────────────────

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
            EquippedSlots = new List<HeroEquippedSlot>
            {
                new()
                {
                    HeroId = heroId,
                    Slot = EquipmentSlotType.RIGHT_HAND,
                    HeroItemCellId = twoHanderCell.Id,
                    HeroItemCell = twoHanderCell,
                },
                new()
                {
                    HeroId = heroId,
                    Slot = EquipmentSlotType.LEFT_HAND,
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

    // ── equip: all slots occupied ─────────────────────────────────────────────

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
            EquippedSlots = new List<HeroEquippedSlot>
            {
                new()
                {
                    HeroId = heroId,
                    Slot = EquipmentSlotType.RIGHT_HAND,
                    HeroItemCellId = cell1.Id,
                    HeroItemCell = cell1,
                },
            },
        };

        var sut = CreateService(hero, userId);

        // weapon2's only allowed slot (RIGHT_HAND) is occupied
        await Assert.ThrowsAsync<ActionDeniedException>(() => sut.EquipAsync(userId, cell2.Id));
    }

    // ── equip: item not owned ─────────────────────────────────────────────────

    [Fact]
    public async Task EquipAsync_ItemNotOwned_Throws()
    {
        var userId = Guid.NewGuid();
        var hero = CreateHero(userId);
        var sut = CreateService(hero, userId);

        await Assert.ThrowsAsync<ActionDeniedException>(() => sut.EquipAsync(userId, Guid.NewGuid()));
    }

    // ── unequip ───────────────────────────────────────────────────────────────

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
            EquippedSlots = new List<HeroEquippedSlot>
            {
                new()
                {
                    HeroId = heroId,
                    Slot = EquipmentSlotType.RIGHT_HAND,
                    HeroItemCellId = cell.Id,
                    HeroItemCell = cell,
                },
            },
        };

        var storage = new FakeEquipmentStorage();
        var sut = CreateService(hero, userId, storage);

        await sut.UnequipAsync(userId, EquipmentSlotType.RIGHT_HAND);

        Assert.True(storage.UnequipCalled);
    }

    [Fact]
    public async Task UnequipAsync_EmptySlot_Throws()
    {
        var userId = Guid.NewGuid();
        var hero = CreateHero(userId);
        var sut = CreateService(hero, userId);

        await Assert.ThrowsAsync<ActionDeniedException>(
            () => sut.UnequipAsync(userId, EquipmentSlotType.RIGHT_HAND));
    }

    // ── fakes ─────────────────────────────────────────────────────────────────

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
        public EquipmentSlotType LastEquippedSlot { get; private set; }

        public Task EquipItemToSlotAsync(Guid heroId, Guid heroItemCellId, EquipmentSlotType slot)
        {
            EquipCalled = true;
            LastEquippedSlot = slot;
            return Task.CompletedTask;
        }

        public Task UnequipItemFromSlotAsync(Guid heroId, EquipmentSlotType slot)
        {
            UnequipCalled = true;
            return Task.CompletedTask;
        }

        public Task<HeroItemCell> ConsumePocketItemAsync(Guid heroId, Guid heroItemCellId) =>
            throw new NotImplementedException();
    }
}
