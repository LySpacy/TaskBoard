using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskBoard.API.Contracts.Project;
using TaskBoard.Domain.Interfaces.Services;
using TaskBoard.Domain.Models;
using TaskBoard.Service.Services;

namespace TaskBoard.API.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectsService _projectsService;
        private readonly IAuthorizationService _authorizationService;
        public ProjectsController(IProjectsService projectsService, IAuthorizationService authorizationService)
        {
            _projectsService = projectsService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [Authorize(Policy = "AllNotBanedUsers")]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _projectsService.GetProjects();

            if (projects.IsSuccess)
            {
                return View(projects.Value);
            }

            return View(projects.Error);
        }

        [HttpGet]
        [Authorize(Policy = "AllNotBanedUsers")]
        public async Task<IActionResult> GetProject(Guid id)
        {

            if (!_authorizationService.AuthorizeAsync(User, null, "AdminOrManagerPolicy").Result.Succeeded)
            {
                var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId").Value);

                var projectWithUserSprints = await _projectsService.GetProjectWithUserSprints(id, userId);

                if (projectWithUserSprints.IsFailure)
                {
                    return View(projectWithUserSprints.Error);
                }

                return View(projectWithUserSprints.Value);
            }
           
            var projectAllSprints = await _projectsService.GetProject(id);
            if (projectAllSprints.IsFailure)
            {
                return View(projectAllSprints.Error);
            }

            return View(projectAllSprints.Value);
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrManagerPolicy")]
        public IActionResult CreateProject() 
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrManagerPolicy")]
        public IActionResult CreateProject(CreateProjectRequest projectRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(projectRequest);
            }

            var project = ProjectModel.Create(Guid.NewGuid(),projectRequest.Title, projectRequest.Description,new List<SprintModel>());

            if (project.IsFailure)
            {
                return BadRequest(project.Error);
            }
            _projectsService.CreateProject(project.Value);
            return RedirectToAction("GetAll");
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrManagerPolicy")]
        public async Task<IActionResult> DeleteProject(Guid id) 
        { 
            await _projectsService.DeleteProject(id);

            return RedirectToAction("GetAll");
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrManagerPolicy")]
        public async Task<IActionResult> EditProject(Guid id) 
        { 
            var project = await _projectsService.GetProject(id);
            if (project.IsSuccess)
            {
                var projectRequest = new UpdateProjectRequest(id, project.Value.Title, project.Value.Description);
                return View(projectRequest);
            }

            return RedirectToAction("GetAll");
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrManagerPolicy")]
        public async Task<IActionResult> EditProject(UpdateProjectRequest projectRequest)
        {
            var project = ProjectModel.Create(
                projectRequest.Id,
                projectRequest.Title,
                projectRequest.Description,
                new List<SprintModel>());

            if (project.IsFailure)
            {
                return View(projectRequest);
            }

            var projectUpdate = await _projectsService.UpdateProject(project.Value);

            if (projectUpdate.IsSuccess) 
            {
                return RedirectToAction("GetAll", projectRequest.Id);
            }

            return View(project);
        }

    }
}
