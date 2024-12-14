using API.GraphQL.Shared;
using Infrastructure.Entities;

namespace API.GraphQL.Subsciptions.Models;

public record WarehouseEvent(EventType EventType, Warehouse Warehouse);