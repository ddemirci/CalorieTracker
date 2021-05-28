using System.Threading.Tasks;
using CalorieTracker.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CalorieTracker.Services.IServices
{
    public interface ITokenAuthenticationService
    {
        Task<string> GenerateJwtTokenAsync(AppUser user);
        Task ValidateJwtTokenAsync(TokenValidatedContext context);
    }
}