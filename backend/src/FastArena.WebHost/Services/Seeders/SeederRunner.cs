namespace FastArena.WebHost.Services.Seeders;

public class SeederRunner
{
    private readonly PortraitSeeder _portraitSeeder;
    private readonly MonsterSeeder _monsterSeeder;
    private readonly EntityLinker _entityLinker;

    public SeederRunner(
        PortraitSeeder portraitSeeder, 
        MonsterSeeder monsterSeeder, 
        EntityLinker entityLinker
    ) {
        _portraitSeeder = portraitSeeder;
        _monsterSeeder = monsterSeeder;
        _entityLinker = entityLinker;
    }

    public async Task RunAsync(string wwwrootPath)
    {
        await _portraitSeeder.SeedAsync(wwwrootPath);
        await _monsterSeeder.SeedAsync();
        await _entityLinker.LinkMonstersToPortraitsAsync();
    }
}
