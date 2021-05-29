using CalorieTracker.Helpers;
using CalorieTracker.Models.Enums;
using CalorieTracker.Models.Requests;
using FluentValidation;

namespace CalorieTracker.Validators
{
    public class UserInformationValidator : AbstractValidator<UserInformationRequest>
    {
        public UserInformationValidator()
        {
            RuleFor(request => request.Gender).IsInEnum().WithMessage(ErrorMessages.GenderIsNotValid.GetDisplayDescription());
            RuleFor(request => request.Height).NotEmpty().WithMessage(ErrorMessages.HeightIsNotValid.GetDisplayDescription());
            RuleFor(request => request.DateOfBirth).NotEmpty().WithMessage(ErrorMessages.DateOfBirthIsNotValid.GetDisplayDescription());
            RuleFor(request => request.Weight).NotEmpty().WithMessage(ErrorMessages.WeightIsNotValid.GetDisplayDescription());
            RuleFor(request => request.MinCalories).NotEmpty().WithMessage(ErrorMessages.MinCaloriesIsNotValid.GetDisplayDescription());
            RuleFor(request => request.MaxCalories).NotEmpty().WithMessage(ErrorMessages.MaxCaloriesIsNotValid.GetDisplayDescription());
        }
    }
}