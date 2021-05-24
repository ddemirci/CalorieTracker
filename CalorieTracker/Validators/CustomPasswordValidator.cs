using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalorieTracker.Helpers;
using CalorieTracker.Models;
using CalorieTracker.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace CalorieTracker.Validators
{
    public class CustomPasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            var errorList = new List<IdentityError>();
            const int charLimit = Constants.MinimumRequiredPasswordLength;

            if(password.Length < charLimit)
                errorList.Add(new IdentityError
                {
                    Code = "PasswordLength",
                    Description = string.Format(ErrorMessages.PasswordLengthMustBeAtLeastX.GetDisplayDescription(),charLimit)
                });
            
            if(password.ToLower().Contains(user.UserName.ToLower()))
                errorList.Add(new IdentityError
                {
                    Code = "PasswordContainsUserName",
                    Description = ErrorMessages.PasswordShouldNotContainUserName.GetDisplayDescription()
                });

            return !errorList.Any()
                ? Task.FromResult(IdentityResult.Success)
                : Task.FromResult(IdentityResult.Failed(errorList.ToArray()));
        }
    }
}