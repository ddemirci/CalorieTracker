using System.Text;
using System.Threading.Tasks;
using CalorieTracker.Services.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CalorieTracker.Extensions
{
    public static class AuthenticationExtensions
    {
        public static void AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(auth =>
                {
                    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = configuration["JwtTokenIssuer"],
                        ValidAudience = configuration["JwtTokenAudience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecretKey"])),
                        ValidateLifetime = false // if true, has to add expiration date to the token
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = _ => Task.CompletedTask,
                        OnTokenValidated = context =>
                        {
                            var tokenValidatorService = context.HttpContext.RequestServices
                                .GetRequiredService<ITokenAuthenticationService>();
                            return tokenValidatorService.ValidateJwtTokenAsync(context);
                        },
                        OnMessageReceived = _ => Task.CompletedTask,
                        OnChallenge = _ => Task.CompletedTask
                    };
                });
        }
    }
}