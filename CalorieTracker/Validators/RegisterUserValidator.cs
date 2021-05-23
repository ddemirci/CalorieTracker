using System.Threading.Tasks;
using CalorieTracker.Helpers;
using CalorieTracker.Models;
using CalorieTracker.Models.Enums;
using CalorieTracker.Models.Requests;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace CalorieTracker.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterUserValidator(UserManager<AppUser> userManager)
        {
            const int minLengthForUserName = 4;
            const int maxLengthForUserName = 15;
            _userManager = userManager;
            
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage(ErrorMessages.EmailCannotBeEmpty.GetDisplayDescription())
                .EmailAddress().WithMessage(ErrorMessages.EmailFormatIsInvalid.GetDisplayDescription())
                .MustAsync((email, _) => EmailNotExists(email)).WithMessage(ErrorMessages.EmailIsAlreadyTaken.GetDisplayDescription());
            
            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage(ErrorMessages.UserNameCannotBeEmpty.GetDisplayDescription())
                .MinimumLength(minLengthForUserName).WithMessage(string.Format(ErrorMessages.UserNameLengthMustBetweenXAndY.GetDisplayDescription(), minLengthForUserName, maxLengthForUserName))
                .MaximumLength(maxLengthForUserName).WithMessage(string.Format(ErrorMessages.UserNameLengthMustBetweenXAndY.GetDisplayDescription(), minLengthForUserName, maxLengthForUserName))
                .MustAsync((userName, _) => UserNameNotExists(userName)).WithMessage(ErrorMessages.UserNameIsAlreadyTaken.GetDisplayDescription());
            
            RuleFor(request => request.FirstName)
                .NotEmpty().WithMessage(ErrorMessages.FirstNameCannotBeEmpty.GetDisplayDescription());

            RuleFor(request => request.LastName)
                .NotEmpty().WithMessage(ErrorMessages.LastNameCannotBeEmpty.GetDisplayDescription());

            RuleFor(request => request.Password)
                .NotEmpty().WithMessage(ErrorMessages.PasswordCannotBeEmpty.GetDisplayDescription());
            
            RuleFor(request => request.PasswordConfirmation)
                .NotEmpty().WithMessage(ErrorMessages.PasswordCannotBeEmpty.GetDisplayDescription())
                .Equal(request => request.Password)
                .WithMessage(ErrorMessages.PasswordsShouldMatch.GetDisplayDescription());

            RuleFor(request => request.PhoneNumber)
                .NotEmpty().WithMessage(ErrorMessages.PhoneNumberCannotBeEmpty.GetDisplayDescription());

            RuleFor(request => request.Gender).IsInEnum();
        }

        private async Task<bool> EmailNotExists(string email) => await _userManager.FindByEmailAsync(email) == null;
        private async Task<bool> UserNameNotExists(string userName) => await _userManager.FindByNameAsync(userName) == null; 
    }
}