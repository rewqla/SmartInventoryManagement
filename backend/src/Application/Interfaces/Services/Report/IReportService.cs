using Application.DTO.Warehouse;

namespace Application.Interfaces.Services.Report;

public interface IReportService
{
    byte[] GenerateReport<T>(IEnumerable<T> items);
}