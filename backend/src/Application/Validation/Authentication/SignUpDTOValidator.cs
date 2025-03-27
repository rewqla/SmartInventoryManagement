using Application.DTO.Authentication;
using FluentValidation;

namespace Application.Validation.Authentication;

public class SignUpDTOValidator : AbstractValidator<SignUpDTO>
{
    public SignUpDTOValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(50).WithMessage("Full name must be at most 50 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^\+?\d{10,15}$").WithMessage("Invalid phone number format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches(@"\d").WithMessage("Password must contain at least one number")
            .Matches(@"[^\w\d\s]").WithMessage("Password must contain at least one special character");
    }
    
    //todo: add check if email and phone number is unique
}