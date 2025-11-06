using Gold_Five.Domain.Catalog;
using Microsoft.EntityFrameworkCore;

namespace Gold_Five.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ModelBuilder builder)
        {
            builder.Entity<Item>().HasData(
            new Item("Shirt", "Ohio State shirt", "Mike", 29.99m)
            {
                Id = 1
            },
            new Item("Shorts", "Ohio State shorts", "Mike", 44.99m)
            {
                Id= 2
            }
            );
        }
    }
}
