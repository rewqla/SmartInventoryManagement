using Application.DTO.Warehouse;
using FluentValidation;
using Infrastructure.Entities;

namespace Application.Validation.Warehouse;

public class WarehouseDTOValidator : AbstractValidator<WarehouseDTO>
{
    public WarehouseDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Location)
            .MinimumLength(3).WithMessage("Location must be at least 3 characters long");
    }
}