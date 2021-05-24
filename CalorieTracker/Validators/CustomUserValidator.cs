using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalorieTracker.Helpers;
using CalorieTracker.Models;
using CalorieTracker.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace CalorieTracker.Validators
{
    /// <summary>
    /// Custom user validator. In addition to Identity User Validation.
    /// Also they can be added to FluentValidator class. For only demo purpose
    /// </summary>
    public class CustomUserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            var errors = new List<IdentityError>();
            if (int.TryParse(user.UserName[0].ToString(), out _)) 
                errors.Add(new IdentityError
                {
                    Code = "UserNameNumberStartWith", 
                    Description = ErrorMessages.UserNameCannotStartWithDigit.GetDisplayDescription()
                });
            return Task.FromResult(!errors.Any() ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}