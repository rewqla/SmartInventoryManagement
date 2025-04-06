using Application.Interfaces.Services.Report;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Reports;

public class ReportService : IReportService
{
    private readonly IServiceProvider _serviceProvider;

    public ReportService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public byte[] GenerateReport<T>(IEnumerable<T> data)
    {
        var generator = _serviceProvider.GetService<IReportGenerator<T>>();
        if (generator == null)
            throw new InvalidOperationException($"No report generator registered for type {typeof(T).Name}");

        return generator.GenerateReport(data);
    }
}