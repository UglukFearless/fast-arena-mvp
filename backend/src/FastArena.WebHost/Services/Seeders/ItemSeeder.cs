using FastArena.Core.Domain.Effects;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Items;
using FastArena.Dal;
using FastArena.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastArena.WebHost.Services.Seeders;

public class ItemSeeder
{
    private readonly ApplicationContext _context;

    public ItemSeeder(ApplicationContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (await _context.Items.AnyAsync())
            return;

        var healingPotionId = Guid.NewGuid();
        var painkillerId = Guid.NewGuid();
        var furyPotionId = Guid.NewGuid();
        var oneHandSwordId = Guid.NewGuid();
        var shieldId = Guid.NewGuid();
        var twoHandSwordId = Guid.NewGuid();

        var items = new List<ItemDal>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Золото",
                Description = "Это золото, дурачок, ты точно знаешь, зачем оно нужно!",
                BaseCost = 1,
                ItemImage = "/assets/items/coin.png",
                CanBeEquipped = false,
                CanBeFolded = true,
                Type = ItemType.MONEY,
            },
            new()
            {
                Id = healingPotionId,
                Name = "Лечебное зелье",
                Description = "От этого точно станет лучше, если не схватить по рылу пока будешь пить. Восстанавливает 60 очков здоровья.",
                BaseCost = 15,
                ItemImage = "/assets/items/posion_red.png",
                CanBeEquipped = true,
                CanBeFolded = false,
                Type = ItemType.POTION,
                Effects = new List<EffectDefinitionDal>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        ItemId = healingPotionId,
                        Type = EffectType.HEAL_HP,
                        DurationRounds = 1,
                        Magnitude = 60,
                        MinValue = 60,
                        MaxValue = 60,
                        ChancePercent = 100,
                        ConditionType = EffectConditionType.ALWAYS,
                        TargetType = EffectTargetType.SELF,
                    }
                },
            },
            new()
            {
                Id = painkillerId,
                Name = "Обезболивающий наркотик",
                Description = "Восстанавливает мастерство до максимума на 3 хода. А ещё с него знатно прёт!",
                BaseCost = 30,
                ItemImage = "/assets/items/posion_violet.png",
                CanBeEquipped = true,
                CanBeFolded = false,
                Type = ItemType.POTION,
                Effects = new List<EffectDefinitionDal>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        ItemId = painkillerId,
                        Type = EffectType.OVERRIDE_ABILITY_TO_MAX,
                        DurationRounds = 3,
                        Magnitude = 0,
                        MinValue = 0,
                        MaxValue = 0,
                        ChancePercent = 100,
                        ConditionType = EffectConditionType.ALWAYS,
                        TargetType = EffectTargetType.SELF,
                    }
                },
            },
            new()
            {
                Id = furyPotionId,
                Name = "Зелье свирепости",
                Description = "Делает все успешные атаки более смертоносными добавляя 2 единицы силы удара на 3 хода. Ещё орёшь, как дурной и ссышься.",
                BaseCost = 30,
                ItemImage = "/assets/items/posion_black.png",
                CanBeEquipped = true,
                CanBeFolded = false,
                Type = ItemType.POTION,
                Effects = new List<EffectDefinitionDal>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        ItemId = furyPotionId,
                        Type = EffectType.STRIKE_POWER_BONUS,
                        DurationRounds = 3,
                        Magnitude = 2,
                        MinValue = 2,
                        MaxValue = 2,
                        ChancePercent = 100,
                        ConditionType = EffectConditionType.ON_SUCCESSFUL_STRIKE,
                        TargetType = EffectTargetType.SELF,
                    }
                },
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Драгоценный камень",
                Description = "Красивый, но бесполезный - как ты. Может кто купит?",
                BaseCost = 100,
                ItemImage = "/assets/items/brilliant.png",
                CanBeEquipped = false,
                CanBeFolded = true,
                Type = ItemType.OTHER,
            },
            new()
            {
                Id = oneHandSwordId,
                Name = "Грубый одноручный меч",
                Description = "Не такой грубый, как твой бывший. Добавлять 2 очка урона за каждую единицу силы нанесённого удара.",
                BaseCost = 30,
                ItemImage = "/assets/items/sword_angle.png",
                CanBeEquipped = true,
                CanBeFolded = false,
                Type = ItemType.WEAPON,
            },
            new()
            {
                Id = shieldId,
                Name = "Легкий деревянный щит",
                Description = "Легкий и деревянный... и щит. Позволяет с вероятностью 20 процентов полностью заблокировать 2 пропущенных атаки. Ломается.",
                BaseCost = 50,
                ItemImage = "/assets/items/shchit.png",
                CanBeEquipped = true,
                CanBeFolded = false,
                Type = ItemType.SHIELD,
            },
            new()
            {
                Id = twoHandSwordId,
                Name = "Грубый двуручный меч",
                Description = "Имено такой грубый, как твой бывший. Добавляет 1 единицу силы атаки при успешном ударе.",
                BaseCost = 40,
                ItemImage = "/assets/items/sword_straight.png",
                CanBeEquipped = true,
                CanBeFolded = false,
                Type = ItemType.WEAPON,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Грибочек",
                Description = "Не стоит его есть.",
                BaseCost = 5,
                ItemImage = "/assets/items/mushroom.png",
                CanBeEquipped = false,
                CanBeFolded = true,
                Type = ItemType.OTHER,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Чертополох",
                Description = "ЧЕРТ опо ЛОХ.",
                BaseCost = 5,
                ItemImage = "/assets/items/flower.png",
                CanBeEquipped = false,
                CanBeFolded = true,
                Type = ItemType.OTHER,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Корешок",
                Description = "Зачем ты его взял?",
                BaseCost = 5,
                ItemImage = "/assets/items/roots.png",
                CanBeEquipped = false,
                CanBeFolded = true,
                Type = ItemType.OTHER,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Коготь",
                Description = "Почётная вещь для любого говноря.",
                BaseCost = 10,
                ItemImage = "/assets/items/kogot.png",
                CanBeEquipped = false,
                CanBeFolded = true,
                Type = ItemType.OTHER,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Зуб",
                Description = "Целенький, без кариеса! Да как так?!",
                BaseCost = 10,
                ItemImage = "/assets/items/teeth.png",
                CanBeEquipped = false,
                CanBeFolded = true,
                Type = ItemType.OTHER,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Петушиное перо",
                Description = "Стоит записать такой шансон трек.",
                BaseCost = 3,
                ItemImage = "/assets/items/gay_feather.png",
                CanBeEquipped = false,
                CanBeFolded = true,
                Type = ItemType.OTHER,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Каменное сердце",
                Description = "Как у твоей бывшей :'(",
                BaseCost = 75,
                ItemImage = "/assets/items/fire_stone.png",
                CanBeEquipped = false,
                CanBeFolded = true,
                Type = ItemType.OTHER,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Рог",
                Description = "Ещё один... А разве у тебя уже нет двух?",
                BaseCost = 20,
                ItemImage = "/assets/items/horn.png",
                CanBeEquipped = false,
                CanBeFolded = true,
                Type = ItemType.OTHER,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Сердце дракона",
                Description = "Банально? Я скажу - классика.",
                BaseCost = 200,
                ItemImage = "/assets/items/heart.png",
                CanBeEquipped = false,
                CanBeFolded = true,
                Type = ItemType.OTHER,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Пыльца пикси",
                Description = "Целый мешочек. Ты откуда и как её вытряс?!",
                BaseCost = 15,
                ItemImage = "/assets/items/powder.png",
                CanBeEquipped = false,
                CanBeFolded = true,
                Type = ItemType.OTHER,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Ледяное Сердце",
                Description = "Не разбей!",
                BaseCost = 100,
                ItemImage = "/assets/items/ice_thing.png",
                CanBeEquipped = false,
                CanBeFolded = true,
                Type = ItemType.OTHER,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Экзоплазма",
                Description = "Будем называть эту субстанцию именно так, но лучше её не нюхать.",
                BaseCost = 30,
                ItemImage = "/assets/items/posion_green.png",
                CanBeEquipped = false,
                CanBeFolded = true,
                Type = ItemType.OTHER,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Шестерёнка",
                Description = "Часть механизма. Большего и не скажешь - ты ведь не инженер!",
                BaseCost = 30,
                ItemImage = "/assets/items/shesteryonka.png",
                CanBeEquipped = false,
                CanBeFolded = true,
                Type = ItemType.OTHER,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Глаз",
                Description = "Да, это опредённо глаз. Остаётся надеется, что кто-то ещё увидит в нём ценность кроме тебя.",
                BaseCost = 10,
                ItemImage = "/assets/items/eye_ball.png",
                CanBeEquipped = false,
                CanBeFolded = true,
                Type = ItemType.OTHER,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Пучок волос",
                Description = "Пучок волос из бороды гнома-хуекрада. Учитывая их специфическую диету, среди людей ходит поверие, что такие волосы делают мужчину сильным ;)",
                BaseCost = 5,
                ItemImage = "/assets/items/beard.png",
                CanBeEquipped = false,
                CanBeFolded = true,
                Type = ItemType.OTHER,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Клык",
                Description = "Не надо на него ничего давать!",
                BaseCost = 10,
                ItemImage = "/assets/items/big_teeth.png",
                CanBeEquipped = false,
                CanBeFolded = true,
                Type = ItemType.OTHER,
            },
        };

        var itemAllowedSlots = new List<ItemAllowedSlotDal>
        {
            new() { ItemId = healingPotionId, Slot = EquipmentSlotType.POCKET_1 },
            new() { ItemId = healingPotionId, Slot = EquipmentSlotType.POCKET_2 },
            new() { ItemId = healingPotionId, Slot = EquipmentSlotType.POCKET_3 },

            new() { ItemId = painkillerId, Slot = EquipmentSlotType.POCKET_1 },
            new() { ItemId = painkillerId, Slot = EquipmentSlotType.POCKET_2 },
            new() { ItemId = painkillerId, Slot = EquipmentSlotType.POCKET_3 },

            new() { ItemId = furyPotionId, Slot = EquipmentSlotType.POCKET_1 },
            new() { ItemId = furyPotionId, Slot = EquipmentSlotType.POCKET_2 },
            new() { ItemId = furyPotionId, Slot = EquipmentSlotType.POCKET_3 },

            new() { ItemId = oneHandSwordId, Slot = EquipmentSlotType.RIGHT_HAND },
            new() { ItemId = shieldId, Slot = EquipmentSlotType.LEFT_HAND },
            new() { ItemId = twoHandSwordId, Slot = EquipmentSlotType.RIGHT_HAND },
            new() { ItemId = twoHandSwordId, Slot = EquipmentSlotType.LEFT_HAND },
        };

        _context.Items.AddRange(items);
        _context.ItemAllowedSlots.AddRange(itemAllowedSlots);
        await _context.SaveChangesAsync();
    }
}
