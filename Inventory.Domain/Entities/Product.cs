
using Ardalis.GuardClauses;
using Riok.Mapperly.Abstractions;

namespace Inventory.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; } 
    public string Name { get; private set; } = default!;
    public int AvailableStock { get; private set; }
    public int ReorderStock { get; private set; }

    public Product() { }

    [MapperConstructor]
    public Product( string name, int availableStock, int reorderStock)
    {
        Validate(name, availableStock, reorderStock);
        Id = Guid.NewGuid();
        Name = name;
        AvailableStock = availableStock;
        ReorderStock = reorderStock;
    }
    public static Product Create(string name, int availableStock, int reorderStock)
            => new( name, availableStock, reorderStock);

    public static void Validate(string name, int availableStock, int reorderStock)
    {
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.Negative(availableStock);
        Guard.Against.Negative(reorderStock);
    }

}
