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

    [Theory]
    [InlineData("", "Phone number is required")]
    [InlineData("12345", "Invalid phone number format")]
    public void Should_HaveError_When_PhoneNumberIsInvalid(string phoneNumber, string expectedError)
    {
        var model = new SignUpDTO { PhoneNumber = phoneNumber };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
            .WithErrorMessage(expectedError);
    }

    [Theory]
    [InlineData("", "Password is required")]
    [InlineData("weak", "Password must be at least 8 characters long")]
    [InlineData("short1A", "Password must be at least 8 characters long")]
    [InlineData("nouppercase1!", "Password must contain at least one uppercase letter")]
    [InlineData("NOLOWERCASE1!", "Password must contain at least one lowercase letter")]
    [InlineData("NoSpecialChar1", "Password must contain at least one special character")]
    public void Should_HaveError_When_PasswordIsInvalid(string password, string expectedError)
    {
        var model = new SignUpDTO { Password = password };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage(expectedError);
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