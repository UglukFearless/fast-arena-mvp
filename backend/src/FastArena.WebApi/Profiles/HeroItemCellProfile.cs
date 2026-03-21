using FastArena.Core.Domain.Heroes;
using FastArena.WebApi.Dtos;

namespace FastArena.WebApi.Profiles;

internal static class HeroItemCellProfile
{
    public static HeroItemCellDto Map(HeroItemCell cell, bool deep = false)
    {
        if (cell == null)
            return null;

        var item = new HeroItemCellDto
        {
            Id = cell.Id,
            Amount = cell.Amount,
            ItemId = cell.ItemId,
            HeroId = cell.HeroId,
            Item = deep ? ItemProfile.Map(cell.Item) : null,
        };

        return item;
    }

    public static List<HeroItemCellDto> Map(List<HeroItemCell> dals, bool deep = false)
        => dals?.ConvertAll(ic => Map(ic, deep));
}
