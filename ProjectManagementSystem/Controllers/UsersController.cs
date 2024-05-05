using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Services.Interfaces;

namespace ProjectManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController
            (
                IUserService userService
            )
        {
            _userService = userService;
        }

        [HttpPost("ChangeRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeUserRole(string userId, string newRole)
        {
            var res = await _userService.UpdateUserRoleAsync(userId, newRole);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetRoles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRoles()
        {
            var res = await _userService.GetUserRoleAsync();
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetUsers")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> GetUsers()
        {
            var res = await _userService.GetUsersAsync();
            return StatusCode(res.StatusCode, res);
        }
    }
}
