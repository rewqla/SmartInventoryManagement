using Application.DTO.Warehouse;

namespace Application.Interfaces.Services.Report;

public interface IReportService <T>
{
    byte[] GenerateReport(IEnumerable<T> items);
}