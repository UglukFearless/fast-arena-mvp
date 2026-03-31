using FastArena.Core.Models;
using FastArena.WebApi.Dtos;

namespace FastArena.WebApi.Profiles;

public class HeroInfoProfile
{
    public static HeroInfoDto Map(HeroInfoModel info)
    {
        if (info == null)
            return null;

        return new HeroInfoDto
        {
            Id = info.Hero.Id,
            Name = info.Hero.Name,
            Sex = info.Hero.Sex,
            Level = info.Hero.Level,
            PortraitUrl = info.Hero.Portrait?.Url,
            MaxHealth = info.Hero.MaxHealth,
            MaxAbility = info.Hero.MaxHealth / 10,
            IsAlive = info.Hero.IsAlive,
            IsInventoryVisible = info.IsInventoryVisible,
            MoneyAmount = info.MoneyAmount,
            Items = HeroItemCellProfile.Map(info.InventoryItems, true),
            Results = MonsterFightProfile.Map(info.Hero.Results, true) ?? new List<MonsterFightResultDto>(),
        };
    }
}
