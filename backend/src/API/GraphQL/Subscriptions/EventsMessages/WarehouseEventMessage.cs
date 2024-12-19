using API.GraphQL.Shared;
using Application.DTO.Warehouse;

namespace API.GraphQL.Subscriptions.EventsMessages;

public record WarehouseEventMessage(EventType EventType, WarehouseDTO Warehouse);