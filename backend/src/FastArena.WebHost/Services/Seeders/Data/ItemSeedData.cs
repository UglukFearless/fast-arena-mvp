using FastArena.Core.Domain.Effects;
using FastArena.Core.Domain.Heroes;
using FastArena.Core.Domain.Items;
using FastArena.Dal.Entities;

namespace FastArena.WebHost.Services.Seeders.Data;

public static class ItemSeedData
{
    public static readonly Guid GoldId             = Guid.Parse("369d5339-b3cd-4878-8422-caec97d52406");
    public static readonly Guid HealingPotionId    = Guid.Parse("2d83d0ce-7486-44d5-bdd1-5c525507a722");
    public static readonly Guid PainkillerPotionId = Guid.Parse("77dea192-4b12-4ce5-99c4-d1aa4dd8c386");
    public static readonly Guid FuryPotionId       = Guid.Parse("2d7318d1-b511-4d83-a4c0-7a1747e25e8d");
    public static readonly Guid GemId              = Guid.Parse("bc6a96be-9810-420c-af61-8e62a08ab869");
    public static readonly Guid OneHandSwordId     = Guid.Parse("85fa4165-2b36-4e86-8796-f8c40d030367");
    public static readonly Guid ShieldId           = Guid.Parse("3334b058-5ed6-4aaf-a3a5-95089e6cbe79");
    public static readonly Guid TwoHandSwordId     = Guid.Parse("dd714d13-0025-45b7-bd55-1480b84f66e8");
    public static readonly Guid MushroomId         = Guid.Parse("1c47add1-59e0-44d4-a7bc-89426f48fe7b");
    public static readonly Guid ThistleId          = Guid.Parse("764dee97-4036-4232-ae6f-1b112fda0a0f");
    public static readonly Guid RootsId            = Guid.Parse("503c5e5d-0bd4-4770-bcb0-32c647a17c6e");
    public static readonly Guid ClawId             = Guid.Parse("7f96f04b-a17e-440c-bff4-ad0e074c81d5");
    public static readonly Guid ToothId            = Guid.Parse("57d24c37-9b34-49cc-8012-697595b605df");
    public static readonly Guid RoosterFeatherId   = Guid.Parse("770cdfb2-060b-479a-8687-ac9e937d2df9");
    public static readonly Guid StoneHeartId       = Guid.Parse("6364da37-dc34-44e2-8fd1-69220a688465");
    public static readonly Guid HornId             = Guid.Parse("3b78f858-ea93-410a-98e0-98c450448183");
    public static readonly Guid DragonHeartId      = Guid.Parse("343a6663-eeb1-4e44-bf11-a22649d9168d");
    public static readonly Guid PixieDustId        = Guid.Parse("5f7a61f1-8179-4923-ba97-16b9155aa911");
    public static readonly Guid IceHeartId         = Guid.Parse("19d384a4-812b-4efd-a397-870109996704");
    public static readonly Guid EctoplasmId        = Guid.Parse("35ff74e7-6a00-4b7a-b181-0d1c8ecf8682");
    public static readonly Guid GearId             = Guid.Parse("543b9899-ff8d-4c17-b3d9-a00391f90ab1");
    public static readonly Guid EyeId              = Guid.Parse("c04cf8e8-3a6b-49fb-96ec-84d6032bccad");
    public static readonly Guid HairBunchId        = Guid.Parse("57a68e11-72c7-40b4-bd88-ae7a7ef0b72f");
    public static readonly Guid FangId             = Guid.Parse("90d4be39-84cf-4b01-a2f9-ff9bb4d86add");

    public static IReadOnlyList<ItemDal> All => new List<ItemDal>
    {
        new()
        {
            Id = GoldId,
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
            Id = HealingPotionId,
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
                    ItemId = HealingPotionId,
                    Type = EffectType.HEAL_HP,
                    DurationRounds = 1,
                    LifetimeType = EffectLifetimeType.RoundBased,
                    SourceType = EffectSourceType.Potion,
                    Magnitude = 60,
                    MinValue = 60,
                    MaxValue = 60,
                    ChancePercent = 100,
                    TargetType = EffectTargetType.SELF,
                }
            },
        },
        new()
        {
            Id = PainkillerPotionId,
            Name = "Обезболивающие",
            Description = "Наркотик восстанавливает мастерство до максимума на 3 хода. А ещё с него знатно прёт!",
            BaseCost = 20,
            ItemImage = "/assets/items/posion_violet.png",
            CanBeEquipped = true,
            CanBeFolded = false,
            Type = ItemType.POTION,
            Effects = new List<EffectDefinitionDal>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ItemId = PainkillerPotionId,
                    Type = EffectType.OVERRIDE_ABILITY_TO_MAX,
                    DurationRounds = 3,
                    LifetimeType = EffectLifetimeType.RoundBased,
                    SourceType = EffectSourceType.Potion,
                    Magnitude = 0,
                    MinValue = 0,
                    MaxValue = 0,
                    ChancePercent = 100,
                    TargetType = EffectTargetType.SELF,
                }
            },
        },
        new()
        {
            Id = FuryPotionId,
            Name = "Зелье свирепости",
            Description = "Делает все успешные атаки более смертоносными добавляя 2 единицы силы удара на 4 хода. Ещё орёшь, как дурной и ссышься.",
            BaseCost = 20,
            ItemImage = "/assets/items/posion_black.png",
            CanBeEquipped = true,
            CanBeFolded = false,
            Type = ItemType.POTION,
            Effects = new List<EffectDefinitionDal>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ItemId = FuryPotionId,
                    Type = EffectType.STRIKE_POWER_BONUS,
                    DurationRounds = 4,
                    LifetimeType = EffectLifetimeType.RoundBased,
                    SourceType = EffectSourceType.Potion,
                    Magnitude = 2,
                    MinValue = 2,
                    MaxValue = 2,
                    ChancePercent = 100,
                    TargetType = EffectTargetType.SELF,
                }
            },
        },
        new()
        {
            Id = GemId,
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
            Id = OneHandSwordId,
            Name = "Грубый одноручный меч",
            Description = "Не такой грубый, как твой бывший. Добавлять 2 очка урона за каждую единицу силы нанесённого удара.",
            BaseCost = 30,
            ItemImage = "/assets/items/sword_angle.png",
            CanBeEquipped = true,
            CanBeFolded = false,
            Type = ItemType.WEAPON,
            Effects = new List<EffectDefinitionDal>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ItemId = OneHandSwordId,
                    Type = EffectType.UNIT_DAMAGE_DELTA,
                    DurationRounds = 0,
                    LifetimeType = EffectLifetimeType.Persistent,
                    SourceType = EffectSourceType.Equipment,
                    Magnitude = 2,
                    MinValue = 2,
                    MaxValue = 2,
                    ChancePercent = 100,
                    TargetType = EffectTargetType.CONTEXT_VALUE,
                }
            },
        },
        new()
        {
            Id = ShieldId,
            Name = "Легкий деревянный щит",
            Description = "Легкий и деревянный... и щит. Позволяет с вероятностью 50 процентов полностью заблокировать 2 пропущенных атаки. Ломается.",
            BaseCost = 50,
            ItemImage = "/assets/items/shchit.png",
            CanBeEquipped = true,
            CanBeFolded = false,
            Type = ItemType.SHIELD,
            Effects = new List<EffectDefinitionDal>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ItemId = ShieldId,
                    Type = EffectType.INCOMING_STRIKE_FULL_BLOCK,
                    DurationRounds = 0,
                    LifetimeType = EffectLifetimeType.Persistent,
                    SourceType = EffectSourceType.Equipment,
                    Magnitude = 2,
                    MinValue = 2,
                    MaxValue = 2,
                    ChancePercent = 50,
                    TargetType = EffectTargetType.SELF,
                }
            },
        },
        new()
        {
            Id = TwoHandSwordId,
            Name = "Грубый двуручный меч",
            Description = "Имено такой грубый, как твой бывший. Добавляет 1 единицу силы атаки при успешном ударе.",
            BaseCost = 40,
            ItemImage = "/assets/items/sword_straight.png",
            CanBeEquipped = true,
            CanBeFolded = false,
            Type = ItemType.WEAPON,
            Effects = new List<EffectDefinitionDal>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ItemId = TwoHandSwordId,
                    Type = EffectType.STRIKE_POWER_BONUS,
                    DurationRounds = 0,
                    LifetimeType = EffectLifetimeType.Persistent,
                    SourceType = EffectSourceType.Equipment,
                    Magnitude = 1,
                    MinValue = 1,
                    MaxValue = 1,
                    ChancePercent = 100,
                    TargetType = EffectTargetType.CONTEXT_VALUE,
                }
            },
        },
        new()
        {
            Id = MushroomId,
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
            Id = ThistleId,
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
            Id = RootsId,
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
            Id = ClawId,
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
            Id = ToothId,
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
            Id = RoosterFeatherId,
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
            Id = StoneHeartId,
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
            Id = HornId,
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
            Id = DragonHeartId,
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
            Id = PixieDustId,
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
            Id = IceHeartId,
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
            Id = EctoplasmId,
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
            Id = GearId,
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
            Id = EyeId,
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
            Id = HairBunchId,
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
            Id = FangId,
            Name = "Клык",
            Description = "Не надо на него ничего давать!",
            BaseCost = 10,
            ItemImage = "/assets/items/big_teeth.png",
            CanBeEquipped = false,
            CanBeFolded = true,
            Type = ItemType.OTHER,
        },
    };

    public static IReadOnlyList<ItemAllowedSlotDal> AllowedSlots => new List<ItemAllowedSlotDal>
    {
        new() { ItemId = HealingPotionId,    Slot = EquipmentSlotType.POCKET_1 },
        new() { ItemId = HealingPotionId,    Slot = EquipmentSlotType.POCKET_2 },
        new() { ItemId = HealingPotionId,    Slot = EquipmentSlotType.POCKET_3 },

        new() { ItemId = PainkillerPotionId, Slot = EquipmentSlotType.POCKET_1 },
        new() { ItemId = PainkillerPotionId, Slot = EquipmentSlotType.POCKET_2 },
        new() { ItemId = PainkillerPotionId, Slot = EquipmentSlotType.POCKET_3 },

        new() { ItemId = FuryPotionId,       Slot = EquipmentSlotType.POCKET_1 },
        new() { ItemId = FuryPotionId,       Slot = EquipmentSlotType.POCKET_2 },
        new() { ItemId = FuryPotionId,       Slot = EquipmentSlotType.POCKET_3 },

        new() { ItemId = OneHandSwordId,     Slot = EquipmentSlotType.RIGHT_HAND },
        new() { ItemId = ShieldId,           Slot = EquipmentSlotType.LEFT_HAND },
        new() { ItemId = TwoHandSwordId,     Slot = EquipmentSlotType.RIGHT_HAND },
        new() { ItemId = TwoHandSwordId,     Slot = EquipmentSlotType.LEFT_HAND },
    };
}
