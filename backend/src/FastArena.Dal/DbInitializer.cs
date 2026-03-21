using FastArena.Core.Domain.Items;
using FastArena.Core.Domain.Monsters;
using FastArena.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastArena.Dal;

internal class DbInitializer(ModelBuilder modelBuilder)
{
    private readonly ModelBuilder _modelBuilder = modelBuilder;
    public void Seed()
    {
        _modelBuilder.Entity<PortraitTagDal>()
            .HasData([
                new PortraitTagDal {
                    Id = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8"),
                    Name = "Heroes",
                },
                new PortraitTagDal {
                    Id = Guid.Parse("ee715853-6a69-42e3-bcbf-1fe7e5deece1"),
                    Name = "Monsters",
                },
            ]);

        _modelBuilder.Entity<PortraitDal>()
            .HasData([
                // heroes
                new PortraitDal {
                    Id = Guid.Parse("ca03a22e-add9-4f0c-ab0b-1424127864c7"),
                    Url = "/img/portraits/barbarian.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("3549fde2-2aae-4103-b937-ecc6c8c9dea4"),
                    Url = "/img/portraits/cleopatra.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("ab62d54d-e544-4f0f-b247-ef311ae0dd6d"),
                    Url = "/img/portraits/clown.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("8ffc7c23-7d62-4b61-b134-a0fd0f520717"),
                    Url = "/img/portraits/cowled.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("13a713cb-49bd-42cc-9938-e2d9989860f8"),
                    Url = "/img/portraits/crowned-skull.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("13a86047-5e3d-4ab5-ad49-27b2e3606a95"),
                    Url = "/img/portraits/devil-mask.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("6d7d4df9-d79e-461f-843c-b8efc8b44ec6"),
                    Url = "/img/portraits/dwarf-face.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("b9bc273c-1fef-4e31-ae09-c581529c82ad"),
                    Url = "/img/portraits/executioner-hood.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("e54138b4-332e-4f76-b5d3-d5dcf1037ddf"),
                    Url = "/img/portraits/female-vampire.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("2f8720e9-acc9-4268-a1e3-7f00b8818ea0"),
                    Url = "/img/portraits/fish-monster.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("b7adcd38-5e15-4aee-a3fb-84316b806398"),
                    Url = "/img/portraits/gluttony.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("a6f196b6-7099-4ea0-85be-79202622ca62"),
                    Url = "/img/portraits/goblin-head.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("19f59869-8dd6-4d05-aeba-4906f186527f"),
                    Url = "/img/portraits/golem-head.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("72eb27c3-4800-4594-b370-0ea56f32791f"),
                    Url = "/img/portraits/monk-face.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("f5916168-1492-425f-9edd-b306cec0b6b4"),
                    Url = "/img/portraits/nun-face.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("2063913d-ad4b-4ee9-af3c-61294da28481"),
                    Url = "/img/portraits/ogre.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("b5f76bc7-8d6f-403b-97ba-1a2436d785e9"),
                    Url = "/img/portraits/orc-head.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("4fb4ca43-9320-44c6-8912-fcfd6b93b874"),
                    Url = "/img/portraits/overlord-helm.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("f1e60967-f0ba-4ee2-96e3-2946ef94e700"),
                    Url = "/img/portraits/pirate-captain.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("4c7af751-485e-4eda-b68a-29d90f873c9a"),
                    Url = "/img/portraits/troll.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("ae72406f-7262-4dab-854c-52248034022d"),
                    Url = "/img/portraits/vampire-dracula.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("fdc1dc4c-1c21-4721-beec-34d111b1643f"),
                    Url = "/img/portraits/witch-face.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("54dbab58-552d-4371-920e-4a14a943e472"),
                    Url = "/img/portraits/wizard-face.svg",
                },
                new PortraitDal {
                    Id = Guid.Parse("c734b894-8a71-4932-8a9d-b8ac95df6199"),
                    Url = "/img/portraits/woman-elf-face.svg",
                },
                // monsters
                new PortraitDal {
                    Id = Guid.Parse("a26b0257-4cdc-474e-8f23-ed405ba3d453"),
                    Url = "/img/creatures/bad-gnome.png",
                },
                new PortraitDal {
                    Id = Guid.Parse("a1cb1075-099e-42b0-aef6-8a934a550ef9"),
                    Url = "/img/creatures/bully-minion.png",
                },
                new PortraitDal {
                    Id = Guid.Parse("0dd2d5b5-1317-4045-ab92-8f98af616a98"),
                    Url = "/img/creatures/evil-minion.png",
                },
                new PortraitDal {
                    Id = Guid.Parse("9f14a10f-6167-47a4-b688-77a4343d2bc3"),
                    Url = "/img/creatures/fairy.png",
                },
                new PortraitDal {
                    Id = Guid.Parse("318aedc3-e35c-4909-bcb0-391e3149ad72"),
                    Url = "/img/creatures/grasping-slug.png",
                },
                new PortraitDal {
                    Id = Guid.Parse("7bb788ae-1223-4958-8372-ebe4887909e8"),
                    Url = "/img/creatures/ice-golem.png",
                },
                new PortraitDal {
                    Id = Guid.Parse("1d817faa-8f7c-497c-8654-b3dc566459e9"),
                    Url = "/img/creatures/megabot.png",
                },
                new PortraitDal {
                    Id = Guid.Parse("4c4f72d9-54d6-40d4-8d6d-60d7874050b9"),
                    Url = "/img/creatures/rock-golem.png",
                },
                new PortraitDal {
                    Id = Guid.Parse("2e32d240-3540-42aa-af19-193796018899"),
                    Url = "/img/creatures/rooster.png",
                },
                new PortraitDal {
                    Id = Guid.Parse("c94f3239-2f01-4352-aac7-515e71f79d83"),
                    Url = "/img/creatures/shambling-zombie.png",
                },
                new PortraitDal {
                    Id = Guid.Parse("b58a27fa-aa30-4799-995a-719909fa0498"),
                    Url = "/img/creatures/spectre.png",
                },
                new PortraitDal {
                    Id = Guid.Parse("379f3511-73d9-408f-92b5-f6b2cad3f2ba"),
                    Url = "/img/creatures/spiked-dragon-head.png",
                },
                new PortraitDal {
                    Id = Guid.Parse("83804826-a501-404d-9282-91430f75473b"),
                    Url = "/img/creatures/troglodyte.png",
                },
                new PortraitDal {
                    Id = Guid.Parse("d9884436-121c-43ea-adfd-b30ac70038d8"),
                    Url = "/img/creatures/werewolf.png",
                },

            ]);

        _modelBuilder.Entity<MonsterMoldDal>()
            .HasData([
                new MonsterMoldDal {
                    Id = Guid.Parse("37337208-10eb-4367-ae01-6c8e389336af"),
                    Name = "Гном-хуекрад",
                    RankLevel = 3,
                    HealthPerLevel = 5,
                    BaseHealth = 50,
                    Sex = MonsterSex.MALE,
                    PortraitId = Guid.Parse("a26b0257-4cdc-474e-8f23-ed405ba3d453"),
                },
                new MonsterMoldDal {
                    Id = Guid.Parse("0affac7f-367d-44de-a0e4-850ba40f5947"),
                    Name = "Здоровяк хуелом",
                    RankLevel = 12,
                    HealthPerLevel = 10,
                    BaseHealth = 120,
                    Sex = MonsterSex.MALE,
                    PortraitId = Guid.Parse("a1cb1075-099e-42b0-aef6-8a934a550ef9"),
                },
                new MonsterMoldDal {
                    Id = Guid.Parse("b9683eb9-abae-416b-9b14-4747aaedce9e"),
                    Name = "Чёрт",
                    RankLevel = 10,
                    HealthPerLevel = 10,
                    BaseHealth = 110,
                    Sex = MonsterSex.MALE,
                    PortraitId = Guid.Parse("0dd2d5b5-1317-4045-ab92-8f98af616a98"),
                },
                new MonsterMoldDal {
                    Id = Guid.Parse("540345bb-e392-4981-84e6-52822696cdab"),
                    Name = "Фея хуевёртка",
                    RankLevel = 3,
                    HealthPerLevel = 5,
                    BaseHealth = 50,
                    Sex = MonsterSex.FEMALE,
                    PortraitId = Guid.Parse("9f14a10f-6167-47a4-b688-77a4343d2bc3"),
                },
                new MonsterMoldDal {
                    Id = Guid.Parse("15eefcb5-5a35-46da-a30e-26b171aa7ab9"),
                    Name = "Жирослизень",
                    RankLevel = 5,
                    HealthPerLevel = 8,
                    BaseHealth = 70,
                    Sex = MonsterSex.MALE,
                    PortraitId = Guid.Parse("318aedc3-e35c-4909-bcb0-391e3149ad72"),
                },
                new MonsterMoldDal {
                    Id = Guid.Parse("adb57c1e-9dc6-4736-a5f8-be2b311fca71"),
                    Name = "Обоссаный голем",
                    RankLevel = 15,
                    HealthPerLevel = 12,
                    BaseHealth = 150,
                    Sex = MonsterSex.MALE,
                    PortraitId = Guid.Parse("7bb788ae-1223-4958-8372-ebe4887909e8"),
                },
                new MonsterMoldDal {
                    Id = Guid.Parse("0b1cb7b4-d983-4853-a65d-b9b2c21f4df9"),
                    Name = "Членобот",
                    RankLevel = 8,
                    HealthPerLevel = 9,
                    BaseHealth = 90,
                    Sex = MonsterSex.MALE,
                    PortraitId = Guid.Parse("1d817faa-8f7c-497c-8654-b3dc566459e9"),
                },
                new MonsterMoldDal {
                    Id = Guid.Parse("198d23a7-4fdf-4092-8ce9-451327d9f36a"),
                    Name = "Калоид",
                    RankLevel = 13,
                    HealthPerLevel = 12,
                    BaseHealth = 140,
                    Sex = MonsterSex.MALE,
                    PortraitId = Guid.Parse("4c4f72d9-54d6-40d4-8d6d-60d7874050b9"),
                },
                new MonsterMoldDal {
                    Id = Guid.Parse("09ac9fd3-59f6-413c-a202-8c4d6892717b"),
                    Name = "Петух",
                    RankLevel = 1,
                    HealthPerLevel = 5,
                    BaseHealth = 30,
                    Sex = MonsterSex.MALE,
                    PortraitId = Guid.Parse("2e32d240-3540-42aa-af19-193796018899"),
                },
                new MonsterMoldDal {
                    Id = Guid.Parse("1bbb40d9-258e-44b6-a9d2-caf941961a15"),
                    Name = "Зомби",
                    RankLevel = 7,
                    HealthPerLevel = 10,
                    BaseHealth = 80,
                    Sex = MonsterSex.MALE,
                    PortraitId = Guid.Parse("c94f3239-2f01-4352-aac7-515e71f79d83"),
                },
                new MonsterMoldDal {
                    Id = Guid.Parse("ab96b719-c7fc-420e-bfd8-beb35cf533b6"),
                    Name = "Призрок",
                    RankLevel = 6,
                    HealthPerLevel = 10,
                    BaseHealth = 70,
                    Sex = MonsterSex.MALE,
                    PortraitId = Guid.Parse("b58a27fa-aa30-4799-995a-719909fa0498"),
                },
                new MonsterMoldDal {
                    Id = Guid.Parse("4e3fb9c4-a451-4cb8-acb1-2030901d8adc"),
                    Name = "Дракон хуеглот",
                    RankLevel = 20,
                    HealthPerLevel = 15,
                    BaseHealth = 200,
                    Sex = MonsterSex.MALE,
                    PortraitId = Guid.Parse("379f3511-73d9-408f-92b5-f6b2cad3f2ba"),
                },
                new MonsterMoldDal {
                    Id = Guid.Parse("9191d55d-2bfc-4aa8-b064-3a909d666ddc"),
                    Name = "Трогладит",
                    RankLevel = 10,
                    HealthPerLevel = 10,
                    BaseHealth = 100,
                    Sex = MonsterSex.MALE,
                    PortraitId = Guid.Parse("83804826-a501-404d-9282-91430f75473b"),
                },
                new MonsterMoldDal {
                    Id = Guid.Parse("1e309681-9a46-4fc6-822f-94f2b8b23a26"),
                    Name = "Ёборотень",
                    RankLevel = 12,
                    HealthPerLevel = 10,
                    BaseHealth = 120,
                    Sex = MonsterSex.MALE,
                    PortraitId = Guid.Parse("d9884436-121c-43ea-adfd-b30ac70038d8"),
                }
            ]);

        _modelBuilder.Entity<PortraitDal>()
            .HasMany(x => x.Tags)
            .WithMany(x => x.Portraits)
            .UsingEntity<Dictionary<Guid, Guid>>(
                "PortraitDalPortraitTagDal",
                 r => r.HasOne<PortraitTagDal>().WithMany().HasForeignKey("TagsId"),
                 l => l.HasOne<PortraitDal>().WithMany().HasForeignKey("PortraitsId"), 
                 je =>
                 {
                     je.HasKey("PortraitsId", "TagsId");
                     je.HasData(
                         // heroes
                        new {
                            PortraitsId = Guid.Parse("ca03a22e-add9-4f0c-ab0b-1424127864c7"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8") 
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("3549fde2-2aae-4103-b937-ecc6c8c9dea4"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("ab62d54d-e544-4f0f-b247-ef311ae0dd6d"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("8ffc7c23-7d62-4b61-b134-a0fd0f520717"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("13a713cb-49bd-42cc-9938-e2d9989860f8"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("13a86047-5e3d-4ab5-ad49-27b2e3606a95"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("6d7d4df9-d79e-461f-843c-b8efc8b44ec6"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("b9bc273c-1fef-4e31-ae09-c581529c82ad"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("e54138b4-332e-4f76-b5d3-d5dcf1037ddf"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("2f8720e9-acc9-4268-a1e3-7f00b8818ea0"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("b7adcd38-5e15-4aee-a3fb-84316b806398"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("a6f196b6-7099-4ea0-85be-79202622ca62"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("19f59869-8dd6-4d05-aeba-4906f186527f"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("72eb27c3-4800-4594-b370-0ea56f32791f"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("f5916168-1492-425f-9edd-b306cec0b6b4"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("2063913d-ad4b-4ee9-af3c-61294da28481"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("b5f76bc7-8d6f-403b-97ba-1a2436d785e9"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("4fb4ca43-9320-44c6-8912-fcfd6b93b874"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("f1e60967-f0ba-4ee2-96e3-2946ef94e700"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("4c7af751-485e-4eda-b68a-29d90f873c9a"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("ae72406f-7262-4dab-854c-52248034022d"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("fdc1dc4c-1c21-4721-beec-34d111b1643f"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("54dbab58-552d-4371-920e-4a14a943e472"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("c734b894-8a71-4932-8a9d-b8ac95df6199"),
                            TagsId = Guid.Parse("a7b1b2c6-fd34-40ea-a0f4-8153230524b8")
                        },

                        // monsters
                        new
                        {
                            PortraitsId = Guid.Parse("a26b0257-4cdc-474e-8f23-ed405ba3d453"),
                            TagsId = Guid.Parse("ee715853-6a69-42e3-bcbf-1fe7e5deece1")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("a1cb1075-099e-42b0-aef6-8a934a550ef9"),
                            TagsId = Guid.Parse("ee715853-6a69-42e3-bcbf-1fe7e5deece1")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("0dd2d5b5-1317-4045-ab92-8f98af616a98"),
                            TagsId = Guid.Parse("ee715853-6a69-42e3-bcbf-1fe7e5deece1")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("9f14a10f-6167-47a4-b688-77a4343d2bc3"),
                            TagsId = Guid.Parse("ee715853-6a69-42e3-bcbf-1fe7e5deece1")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("318aedc3-e35c-4909-bcb0-391e3149ad72"),
                            TagsId = Guid.Parse("ee715853-6a69-42e3-bcbf-1fe7e5deece1")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("7bb788ae-1223-4958-8372-ebe4887909e8"),
                            TagsId = Guid.Parse("ee715853-6a69-42e3-bcbf-1fe7e5deece1")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("1d817faa-8f7c-497c-8654-b3dc566459e9"),
                            TagsId = Guid.Parse("ee715853-6a69-42e3-bcbf-1fe7e5deece1")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("4c4f72d9-54d6-40d4-8d6d-60d7874050b9"),
                            TagsId = Guid.Parse("ee715853-6a69-42e3-bcbf-1fe7e5deece1")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("2e32d240-3540-42aa-af19-193796018899"),
                            TagsId = Guid.Parse("ee715853-6a69-42e3-bcbf-1fe7e5deece1")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("c94f3239-2f01-4352-aac7-515e71f79d83"),
                            TagsId = Guid.Parse("ee715853-6a69-42e3-bcbf-1fe7e5deece1")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("b58a27fa-aa30-4799-995a-719909fa0498"),
                            TagsId = Guid.Parse("ee715853-6a69-42e3-bcbf-1fe7e5deece1")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("379f3511-73d9-408f-92b5-f6b2cad3f2ba"),
                            TagsId = Guid.Parse("ee715853-6a69-42e3-bcbf-1fe7e5deece1")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("83804826-a501-404d-9282-91430f75473b"),
                            TagsId = Guid.Parse("ee715853-6a69-42e3-bcbf-1fe7e5deece1")
                        },
                        new
                        {
                            PortraitsId = Guid.Parse("d9884436-121c-43ea-adfd-b30ac70038d8"),
                            TagsId = Guid.Parse("ee715853-6a69-42e3-bcbf-1fe7e5deece1")
                        }
                     );
                 }
            );

        _modelBuilder.Entity<ItemDal>()
            .HasData(
                [
                    new ItemDal {
                        Id = Guid.Parse("a389a6fc-04a6-44ec-862d-7d433d802a4d"),
                        Name = "Золото",
                        Description = "Это золото, дурачок, ты точно знаешь, зачем оно нужно!",
                        BaseCost = 1,
                        ItemImage = "/img/items/coin.svg",
                        CanBeEquipped = false,
                        CanBeFolded = true,
                        Type = ItemType.MONEY,
                    },
                ]
            );
    }
}
