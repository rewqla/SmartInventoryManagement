using Application.DTO.Warehouse;
using Infrastructure.Entities;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Application.Reports.Templates;

public static class ProductContent
{
    public static void Compose(IContainer container, IEnumerable<Product> products)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(50); // ID
                columns.RelativeColumn();  // Name
                columns.RelativeColumn();  // UnitPrice
                columns.RelativeColumn();  // Description
            });

            table.Header(header =>
            {
                header.Cell().Text("ID").Bold();
                header.Cell().Text("Name").Bold();
                header.Cell().Text("Unit price").Bold();
                header.Cell().Text("Description").Bold();
            });

            foreach (var warehouse in products)
            {
                table.Cell().Text(warehouse.Id.ToString());
                table.Cell().Text(warehouse.Name);
                table.Cell().Text(warehouse.UnitPrice.ToString());
                table.Cell().Text(warehouse.Description);
            }
        });
    }
}