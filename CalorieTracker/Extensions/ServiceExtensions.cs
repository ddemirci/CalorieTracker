using CalorieTracker.Services;
using CalorieTracker.Services.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CalorieTracker.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.TryAddScoped<ITokenAuthenticationService, TokenAuthenticationService>();
            services.TryAddScoped<IAccountService, AccountService>();
        }
    }
}