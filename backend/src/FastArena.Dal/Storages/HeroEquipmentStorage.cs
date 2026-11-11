using FastArena.Core.Domain.Heroes;
using FastArena.Core.Interfaces.Storages;
using FastArena.Dal.Entities;
using FastArena.Dal.Profiles;
using Microsoft.EntityFrameworkCore;

namespace FastArena.Dal.Storages;

public class HeroEquipmentStorage : IHeroEquipmentStorage
{
    private readonly ApplicationContext _context;

    public HeroEquipmentStorage(ApplicationContext context)
    {
        _context = context;
    }

    public async Task EquipItemAsync(Guid heroId, Guid heroItemCellId, IReadOnlyList<Guid> slotIds)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var heroItemCell = await _context.HeroItemCells
            .FirstOrDefaultAsync(i => i.Id == heroItemCellId && i.HeroId == heroId);

        if (heroItemCell == null)
        {
            throw new InvalidOperationException("Hero item is missing during equip application.");
        }

        var hasItemAlreadyEquipped = await _context.HeroSlots
            .AnyAsync(s => s.HeroId == heroId && s.HeroItemCellId == heroItemCellId);

        if (hasItemAlreadyEquipped)
        {
            throw new InvalidOperationException("Hero item is already equipped.");
        }

        var slotRows = await _context.HeroSlots
            .Where(s => s.HeroId == heroId && slotIds.Contains(s.Id))
            .ToListAsync();

        if (slotRows.Count != slotIds.Count || slotRows.Any(s => s.HeroItemCellId.HasValue))
        {
            throw new InvalidOperationException("Target equipment slot is already occupied or missing.");
        }

        foreach (var slotRow in slotRows)
        {
            slotRow.HeroItemCellId = heroItemCellId;
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    public async Task UnequipItemAsync(Guid heroId, Guid heroItemCellId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var slotRows = await _context.HeroSlots
            .Where(s => s.HeroId == heroId && s.HeroItemCellId == heroItemCellId)
            .ToListAsync();

        if (slotRows.Count == 0)
        {
            throw new InvalidOperationException("Target equipment is already empty.");
        }

        foreach (var slotRow in slotRows)
        {
            slotRow.HeroItemCellId = null;
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }
    public async Task<HeroItemCell> ConsumePocketItemAsync(Guid heroId, Guid heroItemCellId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var slotRow = await _context.HeroSlots
            .Include(s => s.HeroItemCell)
            .ThenInclude(ic => ic!.Item)
            .ThenInclude(i => i!.Effects)
            .FirstOrDefaultAsync(s =>
                s.HeroId == heroId &&
                s.HeroItemCellId == heroItemCellId &&
                SlotKeys.IsPocket(s.SlotKey));

        if (slotRow == null || slotRow.HeroItemCell == null)
        {
            throw new InvalidOperationException("Selected item is not equipped in a pocket slot.");
        }

        var consumedSnapshot = HeroItemCellProfiles.Map(slotRow.HeroItemCell, true);

        var itemCell = slotRow.HeroItemCell;
        var occupiedSlotsOfCell = await _context.HeroSlots
            .Where(s => s.HeroId == heroId && s.HeroItemCellId == itemCell.Id)
            .ToListAsync();

        itemCell.Amount -= 1;
        if (itemCell.Amount <= 0)
        {
            foreach (var slotOfCell in occupiedSlotsOfCell)
            {
                slotOfCell.HeroItemCellId = null;
            }

            _context.HeroItemCells.Remove(itemCell);
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return consumedSnapshot;
    }
}