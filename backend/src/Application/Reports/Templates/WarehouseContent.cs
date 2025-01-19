using Application.DTO.Warehouse;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Reports.Templates;

public static class WarehouseContent
{
    public static void Compose(IContainer container, IEnumerable<WarehouseDTO> warehouses)
    {
        container.Column(column =>
        {
            // Table with warehouse details
            column.Item().Table(table =>
            {
                // Define table columns
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(50); // ID
                    columns.RelativeColumn();  // Name
                    columns.RelativeColumn();  // Location
                });

                // Add header row with styles
                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("ID").Bold();
                    header.Cell().Element(CellStyle).Text("Name").Bold();
                    header.Cell().Element(CellStyle).Text("Location").Bold();
                });

                // Add content rows
                foreach (var warehouse in warehouses)
                {
                    table.Cell().Element(CellStyle).Text(warehouse.Id.ToString());
                    table.Cell().Element(CellStyle).Text(warehouse.Name);
                    table.Cell().Element(CellStyle).Text(warehouse.Location);
                }
            });

            // Add item count row
            column.Item().AlignRight().PaddingTop(10).Text($"Total Count: {warehouses.Count()}").FontSize(12).Bold();
        });
    }

    private static IContainer CellStyle(IContainer container)
    {
        return container
            .Border(1) 
            .BorderColor(Colors.Grey.Lighten2)
            .Padding(5) 
            .AlignMiddle() 
            .AlignLeft(); 
    }
}