using Application.DTO.Warehouse;

namespace SmartInventoryManagement.IntegrationTests.Helpers;

public class WarehouseTestFixture : IDisposable
{
    public Guid CreatedWarehouseId { get; set; } = Guid.Empty;

    public WarehouseDTO GetWarehouseDTO(string name = "Test Warehouse", string location = "Test Location")
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