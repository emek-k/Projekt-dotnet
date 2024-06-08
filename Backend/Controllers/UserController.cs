using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseHouse.Data;
using CourseHouse.Models;
using CoursesHouse.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Backend.Dtos.UserDtos;
using System.Security.Claims;
using Backend.Mappers;
using Backend.Dtos;
using api.Extensions;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using CoursesHouse.Dtos.UserDtos;

namespace CourseHouse.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly UserManager<User> _userManager;
        public UserController(IUserRepository userRepo, UserManager<User> userManager)
        {
            _userRepo = userRepo;
            _userManager = userManager;
        }

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var users = await _userRepo.GetAllAsync();
            var usersWithRoles = users.Select(u => u.ToSimpleUserDto()).ToList();
            return Ok(usersWithRoles);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById([FromRoute] string id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            var userRole = await _userRepo.GetUserRolesAsync(user.Id);
            var userDto = user.ToUserDto();
            userDto.Roles = userRole.ToList();

            return Ok(userDto);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromRoute] string id, [FromBody] UserUpdateDto updatedUserDto)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            if (user.Id != id)
            {
                return Forbid();
            }

            var updatedUser = updatedUserDto.ToUserFromUserUpdatedDto();

            var result = await _userRepo.UpdateAsync(id, updatedUser);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            var user = await _userRepo.DeleteAsync(id);
            if (user == null)
                return NotFound(id);

            return NoContent();
        }
        [HttpPut("{id}/password")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(string id, [FromBody] PasswordUpdateDto passwordUpdateDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != id)
            {
                return Forbid("Bad user!");
            }

            var result = await _userRepo.UpdatePasswordAsyncWithValidation(id, passwordUpdateDto.CurrentPassword, passwordUpdateDto.NewPassword);
            if (result)
            {
                return Ok();
            }

            return BadRequest("Password update failed");
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Password changed successfully");
        }
    }


}

