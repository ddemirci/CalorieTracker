using System.Threading.Tasks;
using AutoMapper;
using CalorieTracker.Models;
using CalorieTracker.Models.Requests;
using CalorieTracker.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CalorieTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<AppUser> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
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
            var registerResult = await _userManager.CreateAsync(newUser, request.Password);
            return !registerResult.Succeeded
                ? BadRequest(registerResult.Errors)
                : Created($"User with username {newUser.UserName} and email address {newUser.Email} has been registered successfully", 
                    _mapper.Map<AppUser, UserViewModel>(newUser));
        }
    }
}