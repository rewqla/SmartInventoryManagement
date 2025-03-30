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

    [Theory]
    [InlineData("", "Full name is required")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "Full name must be at most 50 characters")]
    public void Should_HaveError_When_FullNameIsInvalid(string fullName, string expectedError)
    {
        var model = new SignUpDTO { FullName = fullName };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.FullName)
            .WithErrorMessage(expectedError);
    }
    
    [Theory]
    [InlineData("invalid-email", "Invalid email format")]
    [InlineData("", "Email is required")]
    public void Should_HaveError_When_EmailIsInvalid(string email, string expectedError)
    {
        var model = new SignUpDTO { Email = email };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage(expectedError);
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
    public void Should_HaveError_When_PasswordIsEmpty()
    {
        var model = new SignUpDTO { Password = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password is required");
    }

    [Fact]
    public void Should_HaveError_When_PasswordDoesNotMeetRequirements()
    {
        var model = new SignUpDTO { Password = "weak" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must be at least 8 characters long");

        model.Password = "short1A";
        result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must be at least 8 characters long");

        model.Password = "nouppercase1!";
        result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must contain at least one uppercase letter");

        model.Password = "NOLOWERCASE1!";
        result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must contain at least one lowercase letter");

        model.Password = "NoSpecialChar1";
        result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must contain at least one special character");
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