using Application.DTO.Warehouse;
using HotChocolate.Configuration;
using HotChocolate.Data.Filters;
using HotChocolate.Types.Descriptors.Definitions;

namespace API.GraphQL.Filters;

public class WarehouseFilterType : FilterInputType<WarehouseDTO>
{
    protected override void Configure(IFilterInputTypeDescriptor<WarehouseDTO> descriptor)
    {
        descriptor.Ignore(c => c.Id);
        descriptor.Field(f => f.Name).Type<CustomStringOperationFilterInputType>();
        descriptor.Field(f => f.Location).Type<CustomStringOperationFilterInputType>();
        
        base.Configure(descriptor);
    }
}