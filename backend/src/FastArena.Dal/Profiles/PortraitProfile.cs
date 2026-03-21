
using FastArena.Core.Domain;
using FastArena.Dal.Entities;

namespace FastArena.Dal.Profiles;

internal static class PortraitProfile
{
    public static Portrait Map(PortraitDal portraitDal, bool deep = false)
    {
        var portrait = new Portrait
        {
            Id = portraitDal.Id,
            Url = portraitDal.Url,
        };

        return portrait;
    }

    public static List<Portrait> Map(List<PortraitDal> portraits, bool deep = false) 
        => portraits?.ConvertAll(h => Map(h, deep));
}
