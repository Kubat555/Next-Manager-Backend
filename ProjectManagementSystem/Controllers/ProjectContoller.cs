using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Data.DTO;
using ProjectManagement.Data.Models;
using ProjectManagement.Services.Interfaces;

namespace ProjectManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectContoller : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectContoller(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost("CreateProject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProject([FromBody] ProjectCreateDTO projectCreateDTO)
        {
            var newProject = await _projectService.CreateProject(projectCreateDTO);
            var res = await _projectService.AddEmployeeToProject(projectCreateDTO.UserId, newProject.Id);
            if (res.isSuccess)
            {
                return StatusCode(StatusCodes.Status201Created, "Project created!");
            }
            return BadRequest("It was not possible to create a project, check the submitted data!");
        }

        [HttpPut("EditProject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProject(int id, [FromBody] ProjectCreateDTO project)
        {
            var res = await _projectService.EditProject(id, project);
            return StatusCode(res.StatusCode, res);
        }

        [HttpDelete("DeleteProject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var res = await _projectService.DeleteProject(id);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetEmployeeProjects")]
        public async Task<IActionResult> GetEmployeeProjects(string userId)
        {
            var res = await _projectService.GetEmployeeProjects(userId);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost("AddEmployeeToProject")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> AddEmployeeToProject(string userId, int projectId)
        {
            var res = await _projectService.AddEmployeeToProject(userId, projectId);
            return StatusCode(res.StatusCode, res);
        }

        [HttpDelete("DeleteEmployeeFromProject")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteEmployeeFromProject(string userId, int projectId)
        {
            var res = await _projectService.DeleteEmployeeInProject(userId, projectId);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetProjectById")]
        public async Task<IActionResult> GetProjectById(int projectId)
        {
            var res = await _projectService.GetProjectById(projectId);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetReport")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetReport(int projectId)
        {
            var res = await _projectService.GetReport(projectId);
            return StatusCode(res.StatusCode, res);
        }


    }
}
