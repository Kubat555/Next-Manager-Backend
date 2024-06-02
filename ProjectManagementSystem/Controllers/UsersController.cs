﻿using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> ChangeUserRole(string userId, string roleName)
        {
            var res = await _userService.UpdateUserRoleAsync(userId, roleName);
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

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(string id)
        {
            var res = await _userService.GetUserAsync(id);
            return StatusCode(res.StatusCode, res);
        }
    }
}
