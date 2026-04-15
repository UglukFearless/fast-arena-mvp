using FastArena.Core.Domain.Heroes;
using FastArena.Core.Interfaces.Storages;
using FastArena.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastArena.Dal.Storages;

public class HeroEquipmentStorage : IHeroEquipmentStorage
{
    private readonly ApplicationContext _context;

    public HeroEquipmentStorage(ApplicationContext context)
    {
        _context = context;
    }

    public async Task EquipItemToSlotAsync(Guid heroId, Guid heroItemCellId, EquipmentSlotType slot)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var heroItemCell = await _context.HeroItemCells
            .FirstOrDefaultAsync(i => i.Id == heroItemCellId && i.HeroId == heroId);

        if (heroItemCell == null)
        {
            throw new InvalidOperationException("Hero item is missing during equip application.");
        }

        var hasItemAlreadyEquipped = await _context.HeroEquippedSlots
            .AnyAsync(s => s.HeroId == heroId && s.HeroItemCellId == heroItemCellId);

        if (hasItemAlreadyEquipped)
        {
            throw new InvalidOperationException("Hero item is already equipped.");
        }

        var slotRow = await _context.HeroEquippedSlots
            .FirstOrDefaultAsync(s => s.HeroId == heroId && s.Slot == slot);

        if (slotRow != null && slotRow.HeroItemCellId.HasValue)
        {
            throw new InvalidOperationException("Target equipment slot is already occupied.");
        }

        if (slotRow == null)
        {
            _context.HeroEquippedSlots.Add(new HeroEquippedSlotDal
            {
                HeroId = heroId,
                Slot = slot,
                HeroItemCellId = heroItemCellId,
            });
        }
        else
        {
            slotRow.HeroItemCellId = heroItemCellId;
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    public async Task UnequipItemFromSlotAsync(Guid heroId, EquipmentSlotType slot)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var slotRow = await _context.HeroEquippedSlots
            .FirstOrDefaultAsync(s => s.HeroId == heroId && s.Slot == slot);

        if (slotRow == null || !slotRow.HeroItemCellId.HasValue)
        {
            throw new InvalidOperationException("Target equipment slot is already empty.");
        }

        _context.HeroEquippedSlots.Remove(slotRow);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }
}