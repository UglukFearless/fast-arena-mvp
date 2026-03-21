
using FastArena.Core.Domain;
using FastArena.WebApi.Dtos;

namespace FastArena.WebApi.Profiles;

internal static class PortraitProfile
{
    public static PortraitDto Map(Portrait portrait)
    {
        return new PortraitDto
        {
            Id = portrait.Id,
            Url = portrait.Url,
        };
    }

    public static List<PortraitDto> Map(List<Portrait> portraits) => portraits.ConvertAll(Map);
}
