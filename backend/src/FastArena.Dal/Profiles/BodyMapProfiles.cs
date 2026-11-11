using FastArena.Core.Domain.Bodies;
using FastArena.Dal.Entities;

namespace FastArena.Dal.Profiles;

internal static class BodyMapProfiles
{
    public static BodyMap Map(BodyMapDal dal, bool deep = false)
    {
        if (dal == null)
            return null;

        return new BodyMap
        {
            Id = dal.Id,
            Slug = dal.Slug,
            Name = dal.Name,
            Zones = deep && dal.Zones != null
                ? dal.Zones.Select(z => new BodyZone
                {
                    Id = z.Id,
                    BodyMapId = z.BodyMapId,
                    Slug = z.Slug,
                    Name = z.Name,
                    UnitDamage = z.UnitDamage,
                }).ToList()
                : new List<BodyZone>(),
        };
    }
}
