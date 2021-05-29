using System;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using CalorieTracker.Extensions;
using CalorieTracker.Helpers;
using CalorieTracker.Models;
using CalorieTracker.Models.Enums;
using CalorieTracker.Models.Requests;
using CalorieTracker.Models.ViewModels;
using CalorieTracker.Services.IServices;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public UserController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenAuthenticationService tokenAuthenticationService,
            IAccountService accountService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenAuthenticationService = tokenAuthenticationService;
            _accountService = accountService;
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
            var user = await GetUser();
            if (user == null)
                return BadRequest(ErrorMessages.UserNotFound.GetDisplayDescription());

            if (!await _userManager.CheckPasswordAsync(user, request.OldPassword))
                BadRequest(ErrorMessages.WrongPassword.GetDisplayDescription());

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            return !result.Succeeded
                ? BadRequest(result.Errors)
                : Ok("Password changed successfully");
        }

        /// <summary>
        /// User add information to his/her account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = "Member")]
        [HttpPost("Information")]
        public async Task<IActionResult> UserInformation(UserInformationRequest request)
        {
            var user = await GetUser();
            if (user == null)
                return BadRequest(ErrorMessages.UserNotFound.GetDisplayDescription());

            user.UserInformation = _mapper.Map<UserInformation>(request);

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded ? Ok()
                : BadRequest(ErrorMessages.GeneralError.GetDisplayDescription());
        }

        /// <summary>
        /// User updates his/her weight
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        [Authorize(Roles = "Member")]
        [HttpPut("Information/Weight")]
        public async Task<IActionResult> UpdateWeight([FromQuery] decimal weight)
        {
            Guard.Against.NegativeOrZero(weight, nameof(weight));
            var user = await GetUser();
            if (user == null)
                return BadRequest(ErrorMessages.UserNotFound.GetDisplayDescription());

            if (user.UserInformation == null)
                return BadRequest(ErrorMessages.UserInformationIsNull.GetDisplayDescription());

            user.UserInformation.Weight = weight;
            return await UpdateUser(user);
        }
        
        /// <summary>
        /// User updates his/her height
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        [Authorize(Roles = "Member")]
        [HttpPut("Information/Height")]
        public async Task<IActionResult> UpdateHeight([FromQuery] int height)
        {
            Guard.Against.NegativeOrZero(height, nameof(height));
            var user = await GetUser();
            if (user == null)
                return BadRequest(ErrorMessages.UserNotFound.GetDisplayDescription());

            if (user.UserInformation == null)
                return BadRequest(ErrorMessages.UserInformationIsNull.GetDisplayDescription());

            user.UserInformation.Height = height;
            return await UpdateUser(user);
        }

        #region Helper Methods

        private async Task<AppUser> GetUser()
        {
            var username = User.GetUserName();
            if (username == null)
                return null;
            var user = await _accountService.GetUserByUserName(username);
            return user;
        }

        private async Task<IActionResult> UpdateUser(AppUser user)
        {
            user.ModifiedAt = DateTimeOffset.Now;
            var result = await _accountService.SaveAllAsync();
            return result ? Ok() 
                : BadRequest(ErrorMessages.GeneralError.GetDisplayDescription());
        }
        
        #endregion
    }
}