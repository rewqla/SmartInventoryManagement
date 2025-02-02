using FluentAssertions;

namespace SmartInventoryManagement.IntegrationTests.Api.Tests.Reports;

public class ProductReportsTests :
    IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly HttpClient _httpClient;

    public ProductReportsTests(WebApplicationFactory<IApiMarker> appFactory)
    {
        _httpClient = appFactory.CreateClient();
    }

    [Fact]
    public async Task GetProductsReport_ShouldReturnPdfFile_WhenCalled()
    {
        // Act
        var response = await _httpClient.GetAsync("/api/reports/products");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/pdf");

        var content = await response.Content.ReadAsByteArrayAsync();
        content.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task GetProductsReport_ShouldReturnCorrectFileName()
    {
        // Act
        var response = await _httpClient.GetAsync("/api/reports/Products");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var fileName = response.Content.Headers.ContentDisposition?.FileName;
        fileName.Should().NotBeNull();
        fileName.Should().MatchRegex(@"^ProductsReport_\d{8}_\d{4}\.pdf$");
    }
}