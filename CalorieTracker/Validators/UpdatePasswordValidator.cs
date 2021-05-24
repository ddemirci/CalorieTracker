using CalorieTracker.Helpers;
using CalorieTracker.Models.Enums;
using CalorieTracker.Models.Requests;
using FluentValidation;

namespace CalorieTracker.Validators
{
    public class UpdatePasswordValidator : AbstractValidator<UpdatePasswordRequest>
    {
        public UpdatePasswordValidator()
        {
            RuleFor(request => request.OldPassword)
                .NotEmpty().WithMessage(ErrorMessages.PasswordCannotBeEmpty.GetDisplayDescription());
            
            RuleFor(request => request.NewPasswordConfirmation)
                .NotEmpty().WithMessage(ErrorMessages.PasswordCannotBeEmpty.GetDisplayDescription())
                .Equal(request => request.NewPassword).WithMessage(ErrorMessages.PasswordsShouldMatch.GetDisplayDescription());
        }
    }
}