using System;
using System.Threading.Tasks;
using AutoMapper;
using CalorieTracker.Helpers;
using CalorieTracker.Models;
using CalorieTracker.Models.Enums;
using CalorieTracker.Models.Requests;
using CalorieTracker.Models.ViewModels;
using CalorieTracker.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CalorieTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenAuthenticationService _tokenAuthenticationService;
        private readonly IMapper _mapper;

        public UserController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenAuthenticationService tokenAuthenticationService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenAuthenticationService = tokenAuthenticationService;
            _mapper = mapper;
        }

        /// <summary>
        ///  Register a user to the system.
        /// </summary>
        /// <param name="request">RegisterUserRequest</param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody]RegisterUserRequest request)
        {
            var requestValidation = ValidateRequest(ModelState);
            if (requestValidation != null)
                return BadRequest(requestValidation.ErrorMessage);

            var newUser = _mapper.Map<RegisterUserRequest, AppUser>(request);
            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);
            //Adding member role to registered user.
            result = await _userManager.AddToRoleAsync(newUser, "Member");
            return !result.Succeeded
                ? BadRequest(result.Errors)
                : Created($"User with username {newUser.UserName} and email address {newUser.Email} has been registered successfully", 
                    _mapper.Map<AppUser, UserViewModel>(newUser));
        }

        /// <summary>
        /// User logins
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var requestValidation = ValidateRequest(ModelState);
            if (requestValidation != null)
                return BadRequest(requestValidation.ErrorMessage);

            var user = await _userManager.FindByNameAsync(request.Username) ?? await _userManager.FindByEmailAsync(request.Username);
            if (user == null)
                return BadRequest(ErrorMessages.LoginInformationWrong.GetDisplayDescription());                

            var loginResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (loginResult.Succeeded)
            {
                await _userManager.ResetAccessFailedCountAsync(user);
                var token = await _tokenAuthenticationService.GenerateJwtTokenAsync(user);
                return Ok(token);
            }

            if (loginResult.IsLockedOut)
                return BadRequest(string.Format(ErrorMessages.AccountSuspendedUntilX.GetDisplayDescription(),
                    user.LockoutEnd?.ToString("dd MMMM yyyy, h:mm:ss tt")));

            await _userManager.AccessFailedAsync(user);
            return BadRequest(ErrorMessages.LoginInformationWrong.GetDisplayDescription());
        }

        /// <summary>
        /// User updates his/her password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("Password")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordRequest request)
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);
            if (user == null)
                BadRequest("Error");

            if (!await _userManager.CheckPasswordAsync(user, request.OldPassword))
                BadRequest(ErrorMessages.WrongPassword.GetDisplayDescription());

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            return !result.Succeeded
                ? BadRequest(result.Errors)
                : Ok("Password changed successfully");
        }
    }
}