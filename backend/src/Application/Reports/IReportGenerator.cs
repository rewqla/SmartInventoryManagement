namespace Application.Reports;

public interface IReportGenerator<T>
{
    byte[] GenerateReport(IEnumerable<T> data);
}