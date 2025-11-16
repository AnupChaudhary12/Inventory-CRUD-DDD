using System.Data;
using Ardalis.GuardClauses;

namespace Inventory.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public int AvailableStock { get; private set; }
    public int ReorderStock { get; private set; }

    public Product() { }

    private Product(string name, int availableStock, int reorderStock)
    {
        Validate(name, availableStock, reorderStock);
        Id = Guid.NewGuid();
        Name = name;
        AvailableStock = availableStock;
        ReorderStock = reorderStock;
    }

    public static Product Create(string name, int availableStock, int reorderStock)
        => new(name, availableStock, reorderStock);

    public void Update(string? name, int? availableStock, int? reorderStock)
    {
        Validate(name, availableStock, reorderStock);

        if (!string.IsNullOrWhiteSpace(name))
        {
            Name= name;
        }

        if (availableStock.HasValue)
        {
            AvailableStock = availableStock.Value;
        }

        if (reorderStock.HasValue)
        {
            ReorderStock = reorderStock.Value;
        }
    }

    public static void Validate(string? name, int? availableStock, int? reorderStock)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            Guard.Against.NullOrWhiteSpace(name);
        }

        if (availableStock.HasValue)
        {
            Guard.Against.Negative(availableStock.Value);
        }

        if (reorderStock.HasValue)
        {
            Guard.Against.Negative(reorderStock.Value);
        }
    }
}
