using System;
using CalorieTracker.Data;
using CalorieTracker.Helpers;
using CalorieTracker.Models;
using CalorieTracker.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace CalorieTracker.Extensions
{
    public static class IdentityServerExtensions
    {
        public static void AddIdentityServer(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>(_ => 
                {
                    _.Password.RequireNonAlphanumeric = false;
                    _.Password.RequireUppercase = false;
                    _.Lockout.MaxFailedAccessAttempts = Constants.AccessFailedCountThreshold;
                    _.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(Constants.AccountLockoutDuration);
                })
                .AddPasswordValidator<CustomPasswordValidator>()
                .AddUserValidator<CustomUserValidator>()
                .AddEntityFrameworkStores<DbContext>();
        }
    }
}