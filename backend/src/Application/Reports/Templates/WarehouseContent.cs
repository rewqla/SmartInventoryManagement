using Application.DTO.Warehouse;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Application.Reports.Templates;

public static class WarehouseContent
{
    public static void Compose(IContainer container, IEnumerable<WarehouseDTO> warehouses)
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

            foreach (var warehouse in warehouses)
            {
                table.Cell().Text(warehouse.Id.ToString());
                table.Cell().Text(warehouse.Name);
                table.Cell().Text(warehouse.Location);
            }
        });
    }
}