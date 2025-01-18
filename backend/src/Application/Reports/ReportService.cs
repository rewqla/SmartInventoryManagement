using Application.DTO.Warehouse;
using Application.Interfaces.Services.Report;
using Application.Reports.Templates;
using QuestPDF.Fluent;

namespace Application.Reports;

public class ReportService : IReportService
{
    // todo: enhance document template
    // todo: add page number, header padding
    // todo: write unit tests
    // todo: write integration tests
    public byte[] GenerateWarehouseReport(IEnumerable<WarehouseDTO> warehouses)
    {
        var template = new ReportTemplate(warehouses);

        return template.GeneratePdf();
    }
}