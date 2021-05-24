using CalorieTracker.Helpers;
using CalorieTracker.Models.Enums;
using CalorieTracker.Models.Requests;
using FluentValidation;

namespace CalorieTracker.Validators
{
    public class LoginUserValidator : AbstractValidator<LoginUserRequest>
    {
        public LoginUserValidator()
        {
            const int minPasswordLength = Constants.MinimumRequiredPasswordLength;
            
            RuleFor(request => request.Username)
                .NotEmpty().WithMessage(ErrorMessages.UserNameCannotBeEmpty.GetDisplayDescription());
            RuleFor(request => request.Password)
                .MinimumLength(minPasswordLength)
                .WithMessage(string.Format(ErrorMessages.PasswordLengthMustBeAtLeastX.GetDisplayDescription(),
                    minPasswordLength));
        }
    }
}