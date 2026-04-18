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

    public async Task<HeroItemCell> ConsumePocketItemAsync(Guid heroId, Guid heroItemCellId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var slotRow = await _context.HeroEquippedSlots
            .Include(s => s.HeroItemCell)
            .ThenInclude(ic => ic!.Item)
            .ThenInclude(i => i!.Effects)
            .FirstOrDefaultAsync(s =>
                s.HeroId == heroId &&
                s.HeroItemCellId == heroItemCellId &&
                (s.Slot == EquipmentSlotType.POCKET_1 ||
                 s.Slot == EquipmentSlotType.POCKET_2 ||
                 s.Slot == EquipmentSlotType.POCKET_3));

        if (slotRow == null || slotRow.HeroItemCell == null)
        {
            throw new InvalidOperationException("Selected item is not equipped in a pocket slot.");
        }

        var consumedSnapshot = HeroItemCellProfiles.Map(slotRow.HeroItemCell, true);

        slotRow.HeroItemCell.Amount -= 1;
        if (slotRow.HeroItemCell.Amount <= 0)
        {
            _context.HeroItemCells.Remove(slotRow.HeroItemCell);
            slotRow.HeroItemCellId = null;
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return consumedSnapshot;
    }
}