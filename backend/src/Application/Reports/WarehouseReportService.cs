﻿using Application.DTO.Warehouse;
using Application.Interfaces.Services.Report;
using Application.Reports.Templates;
using QuestPDF.Fluent;

namespace Application.Reports;

public class WarehouseReportService : IReportService<WarehouseDTO>
{
    // todo: write unit tests
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