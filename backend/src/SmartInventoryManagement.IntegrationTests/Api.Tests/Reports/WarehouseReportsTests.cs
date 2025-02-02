namespace SmartInventoryManagement.IntegrationTests.Api.Tests.Reports;

// Sends http requests to the Production db by WebApplicationFactory
public class WarehouseReportsTests :
    IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly HttpClient _httpClient;

    public WarehouseReportsTests(WebApplicationFactory<IApiMarker> appFactory)
    {
        _httpClient = appFactory.CreateClient();
    }

    [Fact]
    public async Task GetWarehousesReport_ShouldReturnPdfFile_WhenCalled()
    {
        // Act
        var response = await _httpClient.GetAsync("/api/reports/warehouses");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/pdf");

        var content = await response.Content.ReadAsByteArrayAsync();
        content.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task GetWarehousesReport_ShouldReturnCorrectFileName()
    {
        // Act
        var response = await _httpClient.GetAsync("/api/reports/warehouses");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var fileName = response.Content.Headers.ContentDisposition?.FileName;
        fileName.Should().NotBeNull();
        fileName.Should().MatchRegex(@"^WarehousesReport_\d{8}_\d{4}\.pdf$");
    }
}