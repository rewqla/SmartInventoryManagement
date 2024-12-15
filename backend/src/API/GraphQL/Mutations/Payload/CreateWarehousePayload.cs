namespace API.GraphQL.Mutations.Payload;

public class CreateWarehousePayload
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Location { get; init; }
}