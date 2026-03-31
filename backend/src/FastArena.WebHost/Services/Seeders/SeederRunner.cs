namespace FastArena.WebHost.Services.Seeders;

public class SeederRunner
{
    private readonly PortraitSeeder _portraitSeeder;
    private readonly MonsterSeeder _monsterSeeder;
    private readonly EntityLinker _entityLinker;
    private readonly ItemSeeder _itemSeeder;

    public SeederRunner(
        PortraitSeeder portraitSeeder, 
        MonsterSeeder monsterSeeder,
        ItemSeeder itemSeeder,
        EntityLinker entityLinker
    ) {
        _portraitSeeder = portraitSeeder;
        _monsterSeeder = monsterSeeder;
        _itemSeeder = itemSeeder;
        _entityLinker = entityLinker; 
    }

    public async Task RunAsync(string wwwrootPath)
    {
        await _portraitSeeder.SeedAsync(wwwrootPath);
        await _monsterSeeder.SeedAsync();
        await _entityLinker.LinkMonstersToPortraitsAsync();
        await _itemSeeder.SeedAsync();
    }
}
