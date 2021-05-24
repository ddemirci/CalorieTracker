using CalorieTracker.Models.Requests;
using CalorieTracker.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CalorieTracker.Extensions
{
    public static class ValidationExtensions
    {
        public static void AddValidationServices(this IServiceCollection services)
        {
            services.AddTransient<IValidator<RegisterUserRequest>, RegisterUserValidator>();
            services.AddTransient<IValidator<RoleRequest>, AppRoleValidator>();
            services.AddTransient<IValidator<UpdatePasswordRequest>, UpdatePasswordValidator>();
            services.AddTransient<IValidator<LoginUserRequest>, LoginUserValidator>();            
        }
    }
}