using Application.DTO.Warehouse;
using Application.Interfaces.Services.Report;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Services.Report;

public class ReportService : IReportService
{
    // todo: move license
    // todo: separate document template
    // todo: enhance document template
    // todo: add page number, header padding
    // todo: create new instance class service for each report
    // todo: write unit tests
    // todo: write integration tests
    public byte[] GenerateWarehouseReport(IEnumerable<WarehouseDTO> warehouses)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(20);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontSize(12));
                
                page.Header()
                    .Text("Warehouses Report")
                    .Bold()
                    .FontSize(28)
                    .AlignCenter();
                
                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(50); // ID
                        columns.RelativeColumn();  // Name
                        columns.RelativeColumn();  // Location
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("ID").Bold();
                        header.Cell().Text("Name").Bold();
                        header.Cell().Text("Location").Bold();
                    });

                    foreach (var warehouse in warehouses)
                    {
                        table.Cell().Text(warehouse.Id.ToString());
                        table.Cell().Text(warehouse.Name);
                        table.Cell().Text(warehouse.Location);
                    }
                });
                
                page.Footer()
                    .AlignRight()
                    .Text($"Generated on {DateTime.UtcNow:yyyy-MM-dd HH:mm}")
                    .FontSize(10);
            });
        }).GeneratePdf();
    }
}