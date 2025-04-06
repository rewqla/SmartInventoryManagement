using Application.DTO.Warehouse;
using Application.Interfaces.Services.Report;
using Application.Reports.Templates;
using Infrastructure.Entities;
using QuestPDF.Fluent;

namespace Application.Reports;

public class ProductReportGenerator : IReportGenerator<Product>
{
    public byte[] GenerateReport(IEnumerable<Product> items)
    {
        string title = "Product Report";
         
         var report = new GeneralReportTemplate(title, container =>
         {
             ProductContent.Compose(container, items);
         });
         
         return report.GeneratePdf();
    }
}