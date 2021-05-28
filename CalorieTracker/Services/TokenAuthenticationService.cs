using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CalorieTracker.Models;
using CalorieTracker.Services.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace CalorieTracker.Services
{
    public class TokenAuthenticationService : ITokenAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        public TokenAuthenticationService(IConfiguration configuration, 
            UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        
        public async Task<string> GenerateJwtTokenAsync(AppUser user)
        {
            var claims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.NameId, user.Id),
                new(JwtRegisteredClaimNames.Sub, user.UserName),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Iss, _configuration["JwtTokenIssuer"]),
                new(JwtRegisteredClaimNames.Aud, _configuration["JwtTokenAudience"]),
                new(JwtRegisteredClaimNames.Iat, user.EnrolledAt.ToString())
            };
            
            var userRoles = await _userManager.GetRolesAsync(user);
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)).ToList());
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecretKey"]));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var token = new JwtSecurityToken(null,
                null,
                claims,
                signingCredentials: signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        public async Task ValidateJwtTokenAsync(TokenValidatedContext context)
        {
            if (!(context.Principal?.Identity is ClaimsIdentity claimsIdentity))
            {
                context.Fail("Error");
                return;
            }
            
            if (!claimsIdentity.Claims.Any())
            {
                context.Fail("Token has no claims.");
                return;
            }

            var userEmail = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                context.Fail("Token has no email.");
                return;
            }

            var user = await _userManager.GetUserAsync(context.Principal);
            if (user == null)
            {
                context.Fail("Expired token.");
            }
        }
    }
}