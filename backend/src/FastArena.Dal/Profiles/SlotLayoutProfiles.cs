using FastArena.Core.Domain.Heroes;
using FastArena.Dal.Entities;

namespace FastArena.Dal.Profiles;

internal static class SlotLayoutProfiles
{
    public static SlotLayout Map(SlotLayoutDal dal, bool deep = false)
    {
        if (dal == null)
            return null;

        return new SlotLayout
        {
            Id = dal.Id,
            Name = dal.Name,
            Entries = deep && dal.Entries != null
                ? dal.Entries.Select(e => new SlotLayoutEntry
                {
                    SlotLayoutId = e.SlotLayoutId,
                    SlotKey = e.SlotKey,
                    Count = e.Count,
                }).ToList()
                : new List<SlotLayoutEntry>(),
        };
    }
}
