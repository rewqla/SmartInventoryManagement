namespace API.Endpoints.Constants;

internal static class InventoryEndpoints
{
    private const string BaseRoute = "api/inventories";

    internal const string GetAll = $"{BaseRoute}/";
    internal const string GetHistory = $"{BaseRoute}/history/{{id:guid}}";
    internal const string GetByWarehouse = $"{BaseRoute}/warehouse/{{warehouseId:guid}}";
    internal const string Create = $"{BaseRoute}/";
    internal const string Update = $"{BaseRoute}/{{id:guid}}";
    internal const string Delete = $"{BaseRoute}/{{id:guid}}";
    internal const string Release = $"{BaseRoute}/{{id:guid}}/release";
}
