using System;
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
    public class RoleController : BaseController
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<AppRole> roleManager, 
            IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Create a new role.
        /// </summary>
        /// <param name="request">RoleRequest</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleRequest request)
        {
            var requestValidation = ValidateRequest(ModelState);
            if (requestValidation != null)
                return BadRequest(requestValidation.ErrorMessage);
            
            var newRole = new AppRole
            {
                Name = request.RoleName,
                CreatedAt = DateTimeOffset.Now,
            };

            var result = await _roleManager.CreateAsync(newRole);
            return !result.Succeeded
                ? BadRequest(result.Errors)
                : Created($"{newRole.Name} role has been created successfully",
                    _mapper.Map<AppRole, RoleViewModel>(newRole));
        }

        /// <summary>
        /// Update a role.
        /// </summary>
        /// <param name="id">RoleId</param>
        /// <param name="request">RoleRequest</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] RoleRequest request)
        {
            var requestValidation = ValidateRequest(ModelState);
            if (requestValidation != null)
                return BadRequest(requestValidation.ErrorMessage);

            var role = await _roleManager.FindByIdAsync(id);
            if(role == null)
                return BadRequest("Specified Role could not be found.");

            role.Name = request.RoleName;
            var result = await _roleManager.UpdateAsync(role);
            
            return !result.Succeeded
                ? BadRequest(result.Errors)
                : Ok($"{role.Name} role has been updated successfully");
        }
    }
}