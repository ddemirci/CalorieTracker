using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace CalorieTracker.Extensions
{
    public static class AuthorizationExtensions
    {
        public static void AddAuthorizationServices(this IServiceCollection services)
        {
            services.AddAuthorization();
        }
    }
}