using System.Threading.Tasks;
using CalorieTracker.Helpers;
using CalorieTracker.Models;
using CalorieTracker.Models.Enums;
using CalorieTracker.Models.Requests;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace CalorieTracker.Validators
{
    public class AppRoleValidator : AbstractValidator<RoleRequest>
    {
        private readonly RoleManager<AppRole> _roleManager;

        public AppRoleValidator(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
            
            RuleFor(request => request.RoleName)
                .NotEmpty().WithMessage(ErrorMessages.RoleNameCannotBeEmpty.GetDisplayDescription())
                .MustAsync((roleName, _) => RoleNameNotExist(roleName))
                .WithMessage(ErrorMessages.RoleAlreadyCreated.GetDisplayDescription());
        }

        private async Task<bool> RoleNameNotExist(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName) == null;
        }
    }
}