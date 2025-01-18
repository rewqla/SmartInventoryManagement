using Application.DTO.Warehouse;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Reports.Templates;

public class ReportTemplate : IDocument
{
    private IEnumerable<WarehouseDTO> Warehouses;

    public ReportTemplate(IEnumerable<WarehouseDTO> warehouses)
    {
        Warehouses = warehouses;
    }

    // todo: create report model wit title, report body and data
    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);
            
                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().Element(ComposeFooter);

            });
    }
    
    //Change document title
    void ComposeHeader(IContainer container)
    {
        container.AlignCenter().ShowOnce().Text("Warehouses Report")
            .FontSize(28).Bold().Italic();
    }
    
    void ComposeFooter(IContainer container)
    {
        container.AlignCenter().Text(x =>
        {
            x.CurrentPageNumber();
            x.Span(" / ");
            x.TotalPages();
        });
    }
    
    void ComposeContent(IContainer container)
    {
        container.Table(table =>
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

            foreach (var warehouse in Warehouses)
            {
                table.Cell().Text(warehouse.Id.ToString());
                table.Cell().Text(warehouse.Name);
                table.Cell().Text(warehouse.Location);
            }
        });
    }
}