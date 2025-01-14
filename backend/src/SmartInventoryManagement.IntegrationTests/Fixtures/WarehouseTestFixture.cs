using Application.DTO.Warehouse;

namespace SmartInventoryManagement.IntegrationTests.Fixtures;

public class WarehouseTestFixture : IDisposable
{
    public Guid CreatedWarehouseId { get; set; } = Guid.Empty;

    public static WarehouseDTO GetWarehouseDTO(string name = "Test Warehouse", string location = "Test Location")
    {
        return new WarehouseDTO
        {
            Name = name,
            Location = location
        };
    }

    public void Dispose()
    {
    }
}