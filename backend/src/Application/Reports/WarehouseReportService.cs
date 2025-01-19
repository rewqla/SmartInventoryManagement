using Application.DTO.Warehouse;
using Application.Interfaces.Services.Report;
using Application.Reports.Templates;
using QuestPDF.Fluent;

namespace Application.Reports;

public class WarehouseReportService : IReportService<WarehouseDTO>
{
    // todo: enhance document template
    // todo: multiple class objects
    // todo: write unit tests
    // todo: write integration tests
    // todo: separate report for each instance and use enum to check report type
    // optional todo: create report factory
    public byte[] GenerateReport(IEnumerable<WarehouseDTO> items)
    {
        string title = "Warehouses Report";
         
         var report = new GeneralReportTemplate(title, container =>
         {
             WarehouseContent.Compose(container, items);
         });
         
         return report.GeneratePdf();
    }
}