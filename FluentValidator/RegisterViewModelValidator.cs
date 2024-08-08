using FluentValidation;
using UserManagementAPI.ViewModels;

namespace UserManagementAPI.Validators
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
                                 .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
                                    .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                                    .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                                    .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                                    .Matches("[0-9]").WithMessage("Password must contain at least one number")
                                    .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");

            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords do not match");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required")
                                       .Matches("^[0-9]{10}$").WithMessage("Invalid phone number");  // Esempio di validazione per numero di telefono
        }
    }
}
