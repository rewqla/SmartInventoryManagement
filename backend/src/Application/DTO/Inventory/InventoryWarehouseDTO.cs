namespace Application.DTO.Inventory;

public class InventoryWarehouseDTO
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
}
