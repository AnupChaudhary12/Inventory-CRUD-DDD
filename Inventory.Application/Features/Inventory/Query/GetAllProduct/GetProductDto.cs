namespace Inventory.Application.Features.Inventory.Query.GetAllProduct;

public class GetProductDto
{
    public Guid Id { get;  set; } 
    public string Name { get;  set; } = default!;
    public int AvailableStock { get; set; }
    public int ReorderStock { get; set; }
}
