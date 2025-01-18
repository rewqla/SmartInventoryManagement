using Application.DTO.Warehouse;

namespace Application.Interfaces.Services.Report;

public interface IReportService
{
    byte[] GenerateWarehouseReport(IEnumerable<WarehouseDTO> warehouses);
}