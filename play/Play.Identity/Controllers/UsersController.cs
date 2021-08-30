using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Play.Identity.Dtos;
using Play.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace Play.Identity.Controllers
{
    [Authorize(Policy = LocalApi.PolicyName)]
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> Get()
        {
            return Ok(_userManager.Users
                                  .ToList()
                                  .Select(user => user.AsDto()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null) return NotFound();
            return user.AsDto();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateUserDto updateUser)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null) return NotFound();

            user.Email = updateUser.Email;
            user.UserName = updateUser.Email;
            user.Gil = updateUser.Gil;

            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null) return NotFound();
            await _userManager.DeleteAsync(user);

            return NotFound();
        }
    }
}

// role-base authorization 
