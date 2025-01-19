using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Application.Reports.Templates;

public class GeneralReportTemplate : IDocument
{
    private readonly string _title;
    private readonly Action<IContainer> _composeContent;

    public GeneralReportTemplate(string title, Action<IContainer> composeContent)
    {
        _title = title;
        _composeContent = composeContent;
    }

    // todo: create report model wit title, report body and data
    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);

                page.Header().Element(ComposeHeader);
                page.Content().Element(_composeContent);
                page.Footer().Element(ComposeFooter);
            });
    }

    //Change document title
    void ComposeHeader(IContainer container)
    {
        container.AlignCenter().ShowOnce().Height(50).Text(_title)
            .FontSize(28).Bold().Italic();
    }

    void ComposeFooter(IContainer container)
    {
        container.AlignRight().Height(50).PaddingRight(10).PaddingTop(5).Text(x =>
        {
            x.Span("Page ");
            x.CurrentPageNumber();
            x.Span("/");
            x.TotalPages();
        });
    }
}