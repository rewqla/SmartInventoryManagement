namespace SmartInventoryManagement.IntegrationTests.Fixtures;

public class WarehouseTestFixture
{
    public Guid CreatedWarehouseId { get; set; } = Guid.Empty;
    public bool isUpdated { get; set; } = false;

    public static WarehouseDTO GetWarehouseDTO(string name = "Test Warehouse", string location = "Test Location")
    {
        return new WarehouseDTO
        {
            Name = name,
            Location = location
        };
    }
}