using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Persistence;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
    {
        
    }
    public DbSet<Product> Products { get; set; }
}
