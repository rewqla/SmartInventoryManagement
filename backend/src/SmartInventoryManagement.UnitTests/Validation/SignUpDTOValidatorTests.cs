using Application.DTO.Authentication;
using Application.Validation.Authentication;
using FluentValidation.TestHelper;

namespace SmartInventoryManagement.Tests.Validation;

public class SignUpDTOValidatorTests
{
    private readonly SignUpDTOValidator _validator;

    public SignUpDTOValidatorTests()
    {
        _validator = new SignUpDTOValidator();
    }

    [Fact]
    public void Should_HaveError_When_FullNameIsEmpty()
    {
        var model = new SignUpDTO { FullName = "" };
        var result = _validator.TestValidate(model);
        
        result.ShouldHaveValidationErrorFor(x => x.FullName)
            .WithErrorMessage("Full name is required");
    }

    [Fact]
    public void Should_NotHaveError_When_AllFieldsAreValid()
    {
        var model = new SignUpDTO
        {
            FullName = "John Doe",
            Email = "johndoe@example.com",
            PhoneNumber = "+380923291424",
            Password = "ValidPassword1!"
        };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}