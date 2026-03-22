using FastArena.Core.Domain.Items;
using FastArena.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastArena.Dal;

internal class DbInitializer(ModelBuilder modelBuilder)
{
    private readonly ModelBuilder _modelBuilder = modelBuilder;
    public void Seed()
    {
        _modelBuilder.Entity<ItemDal>()
            .HasData(
                new ItemDal {
                    Id = Guid.Parse("a389a6fc-04a6-44ec-862d-7d433d802a4d"),
                    Name = "Золото",
                    Description = "Это золото, дурачок, ты точно знаешь, зачем оно нужно!",
                    BaseCost = 1,
                    ItemImage = "/img/items/coin.svg",
                    CanBeEquipped = false,
                    CanBeFolded = true,
                    Type = ItemType.MONEY,
                }
            );
    }
}
