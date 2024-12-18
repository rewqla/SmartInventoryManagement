namespace API.GraphQL.Mutations.Payload;

public record UpdateWarehousePayload(Guid Id, string Name, string Location);