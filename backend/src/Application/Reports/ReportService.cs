using Application.DTO.Warehouse;
using Application.Interfaces.Services.Report;
using Application.Reports.Templates;
using QuestPDF.Fluent;

namespace Application.Reports;

public class ReportService : IReportService
{
    // todo: enhance document template
    // todo: write unit tests
    // todo: write integration tests
    // todo: separate report for each instance and use enum to check report type
    // optional todo: create report factory
    public byte[] GenerateWarehouseReport(IEnumerable<WarehouseDTO> warehouses)
    {
        var report = new GeneralReportTemplate(container =>
        {
            WarehouseContent.Compose(container, warehouses);
        });

        return report.GeneratePdf();
    }
}