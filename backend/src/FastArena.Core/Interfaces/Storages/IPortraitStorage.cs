using FastArena.Core.Domain;

namespace FastArena.Core.Interfaces.Storages;

public interface IPortraitStorage
{
    Task<IList<Portrait>> GetByTagName(string tagName);
}
