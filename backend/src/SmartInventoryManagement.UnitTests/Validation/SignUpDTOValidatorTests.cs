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
    public void Should_HaveError_When_FullNameExceedsMaxLength()
    {
        var model = new SignUpDTO { FullName = new string('a', 51) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.FullName)
            .WithErrorMessage("Full name must be at most 50 characters");
    }

    [Fact]
    public void Should_HaveError_When_EmailIsEmpty()
    {
        var model = new SignUpDTO { Email = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email is required");
    }

    [Fact]
    public void Should_HaveError_When_EmailIsInvalid()
    {
        var model = new SignUpDTO { Email = "invalid-email" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Invalid email format");
    }

    [Fact]
    public void Should_HaveError_When_PhoneNumberIsEmpty()
    {
        var model = new SignUpDTO { PhoneNumber = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
            .WithErrorMessage("Phone number is required");
    }

    [Fact]
    public void Should_HaveError_When_PhoneNumberIsInvalid()
    {
        var model = new SignUpDTO { PhoneNumber = "12345" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
            .WithErrorMessage("Invalid phone number format");
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