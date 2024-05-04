using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Data.DTO;
using ProjectManagement.Services.Interfaces;

namespace ProjectManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectContoller : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectContoller(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost("CreateProject")]
        public async Task<IActionResult> CreateProject(ProjectCreateDTO projectCreateDTO)
        {
            var newProject = await _projectService.CreateProject(projectCreateDTO);
            var res = await _projectService.AddEmployeeToProject(projectCreateDTO.UserId, newProject.Id);
            if (res)
            {
                return StatusCode(StatusCodes.Status201Created, "Project created!");
            }
            return BadRequest("Itwas not possible to create a project, check the submitted data!");
        }
    }
}
