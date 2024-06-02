using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Data.DTO;
using ProjectManagement.Data.Models;
using ProjectManagement.Services.Interfaces;
using ProjectManagement.Services.Services;
using System.Data;

namespace ProjectManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("GetAllTasks")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAllTasks(int projectId)
        {
            var res = await _taskService.GetAllTasks(projectId);
            return StatusCode(res.StatusCode, res);
        }
        [HttpGet("GetEmployeeTasks")]
        public async Task<IActionResult> GetEmployeeTasks(int projectId, string employeeId)
        {
            var res = await _taskService.GetEmployeeTasks(projectId, employeeId);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost("CreateTask")]
        public async Task<IActionResult> CreateTask([FromBody] TaskCreateDTO taskCreateDTO)
        {
            var res = await _taskService.CreateTask(taskCreateDTO);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPut("EditTask")]
        public async Task<IActionResult> EditTask(int id, [FromBody] TaskCreateDTO task)
        {
            var res = await _taskService.EditTask(id, task);
            return StatusCode(res.StatusCode, res);
        }

        [HttpDelete("DeleteTask")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var res = await _taskService.DeleteTask(id);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetStatuses")]
        public async Task<IActionResult> GetStatuses()
        {
            var res = await _taskService.GetAllStatuses();
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetPriorities")]
        public async Task<IActionResult> GetPriorities()
        {
            var res = await _taskService.GetAllPriorities();
            return StatusCode(res.StatusCode, res);
        }
    }
}
