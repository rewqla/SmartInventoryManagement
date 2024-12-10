namespace API.GraphQL.Mutations.Inputs;

public record UpdateWarehouseInput(Guid Id, string Name, string Location);