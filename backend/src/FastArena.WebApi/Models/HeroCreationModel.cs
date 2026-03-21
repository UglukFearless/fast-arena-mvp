using FastArena.Core.Domain.Heroes;

namespace FastArena.WebApi.Models;

public record HeroCreationModel(string Name, HeroSex Sex, Guid PortraitId);
