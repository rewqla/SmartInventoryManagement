using Application.DTO.Warehouse;
using HotChocolate.Configuration;
using HotChocolate.Data.Filters;
using HotChocolate.Data.Sorting;
using HotChocolate.Types.Descriptors.Definitions;

namespace API.GraphQL.Sorting;

public class WarehouseSortType : SortInputType<WarehouseDTO>
{
    protected override void Configure(ISortInputTypeDescriptor<WarehouseDTO> descriptor)
    {
        descriptor.Ignore(c => c.Id);
        descriptor.Field(c => c.Name);
        descriptor.Field(c => c.Location);

        base.Configure(descriptor);
    }
}