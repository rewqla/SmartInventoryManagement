using Application.DTO.Warehouse;

namespace SmartInventoryManagement.IntegrationTests.Helpers;

public static class WarehouseTestHelper
{
    public static Guid CreatedWarehouseId { get; set; } = Guid.Empty;

    public static WarehouseDTO GetWarehouseDTO(
        string name = "Test Warehouse",
        string location = "Test Location")
    {

        return new WarehouseDTO
        {
            Name = name,
            Location = location
        };
    }
}