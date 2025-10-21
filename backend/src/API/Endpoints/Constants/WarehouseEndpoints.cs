namespace API.Endpoints.Constants;

internal static class WarehouseEndpoints
{
    private const string BaseRoute = "api/warehouses";

    internal const string GetAll = $"{BaseRoute}/";
    internal const string GetWithInventories = $"{BaseRoute}/{{id:guid}}/with-inventories";
    internal const string GetById = $"{BaseRoute}/{{id:guid}}";
    internal const string Create = $"{BaseRoute}/";
    internal const string Update = $"{BaseRoute}/{{id:guid}}";
    internal const string Delete = $"{BaseRoute}/{{id:guid}}";
}
