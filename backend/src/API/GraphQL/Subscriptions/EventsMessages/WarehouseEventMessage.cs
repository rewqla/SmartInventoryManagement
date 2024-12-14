using API.GraphQL.Shared;
using Infrastructure.Entities;

namespace API.GraphQL.Subscriptions.EventsMessages;

public record WarehouseEventMessage(EventType EventType, Warehouse Warehouse);