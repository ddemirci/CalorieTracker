using CalorieTracker.Models.Requests;
using CalorieTracker.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CalorieTracker.Extensions
{
    public static class ValidationExtensions
    {
        public static IServiceCollection AddValidationServices(this IServiceCollection services)
        {
            services.AddTransient<IValidator<RegisterUserRequest>, RegisterUserValidator>();
            return services;
        }
    }
}