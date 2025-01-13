using Application.DTO.Warehouse;

namespace SmartInventoryManagement.IntegrationTests.Helpers;

public static class WarehouseTestHelper
{
    public static Guid CreatedWarehouseId { get; set; } = Guid.Empty;

    public static WarehouseDTO GetWarehouseDTO(
        string name = "Test Warehouse",
        string location = "Test Location")
    {
        var id = CreatedWarehouseId != Guid.Empty ? CreatedWarehouseId : Guid.NewGuid();

        return new WarehouseDTO
        {
            Id = id,
            Name = name,
            Location = location
        };
    }
}