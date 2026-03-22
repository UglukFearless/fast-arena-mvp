using FastArena.Dal;
using FastArena.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastArena.WebHost.Services.Seeders;

public class PortraitSeeder
{
    private readonly IDbContextFactory<ApplicationContext> _contextFactory;

    public PortraitSeeder(IDbContextFactory<ApplicationContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task SeedAsync(string wwwrootPath)
    {
        await SeedCategoryAsync(Path.Combine(wwwrootPath, "assets", "portraits"), "Heroes");
        await SeedCategoryAsync(Path.Combine(wwwrootPath, "assets", "creatures"), "Monsters");
    }

    private async Task SeedCategoryAsync(string folderPath, string tagName)
    {
        if (!Directory.Exists(folderPath))
            return;

        var files = Directory.GetFiles(folderPath);
        if (files.Length == 0)
            return;

        await using var context = await _contextFactory.CreateDbContextAsync();

        var existingTag = await context.PortraitTags
            .Include(t => t.Portraits)
            .FirstOrDefaultAsync(t => t.Name == tagName);

        if (existingTag == null)
        {
            existingTag = new PortraitTagDal
            {
                Id = Guid.NewGuid(),
                Name = tagName,
                Portraits = new List<PortraitDal>()
            };
            context.PortraitTags.Add(existingTag);
            await context.SaveChangesAsync();
        }

        existingTag.Portraits ??= new List<PortraitDal>();

        if (existingTag.Portraits.Count > 0)
            return;

        var folderName = Path.GetFileName(folderPath);
        var portraits = files.Select(file => new PortraitDal
        {
            Id = Guid.NewGuid(),
            Url = $"/assets/{folderName}/{Path.GetFileName(file)}",
        }).ToList();

        context.Portraits.AddRange(portraits);
        await context.SaveChangesAsync();

        foreach (var portrait in portraits)
        {
            existingTag.Portraits.Add(portrait);
        }

        await context.SaveChangesAsync();
    }
}
