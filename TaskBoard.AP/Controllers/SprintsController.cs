using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskBoard.API.Contracts.Sprint;
using TaskBoard.Domain.Interfaces.Services;
using TaskBoard.Domain.Models;

namespace TaskBoard.API.Controllers
{
    public class SprintsController : Controller
    {

        private readonly ISprintsService _sprintsService;
        private readonly IUsersService _usersService;
        public SprintsController(ISprintsService sprintsService, IUsersService usersService)
        {
            _sprintsService = sprintsService;
            _usersService = usersService;   
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrManagerPolicy")]
        public IActionResult CreateSprint(Guid idProject)
        {
            var sprintRequest = new CreateSprintRequest(
                idProject, 
                string.Empty, 
                string.Empty, 
                DateTime.Now ,
                string.Empty);

            return View(sprintRequest);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrManagerPolicy")]
        public async Task<IActionResult> CreateSprint(CreateSprintRequest sprintRequest)
        {
            var sprint = SprintModel.Create(
                sprintRequest.ProjectId, 
                Guid.Empty, 
                sprintRequest.Title, 
                sprintRequest.Description, 
                sprintRequest.DateEnd, 
                sprintRequest.Comment, 
                Guid.Empty);

            if (sprint.IsFailure)
            {
                return View(sprint);
            }

            await _sprintsService.CreateSprint(sprint.Value);

            return RedirectToAction("GetProject", "Projects", new { id = sprintRequest.ProjectId });
        }

        [HttpGet]
        [Authorize(Policy = "AllNotBanedUsers")]
        public async Task<IActionResult> GetSprint(Guid id)
        {

            var sprint = await _sprintsService.GetSprint(id);

            if (sprint.IsSuccess)
            {
                return View(sprint.Value);
            }

            return View(sprint.Error);
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrManagerPolicy")]
        public async Task<IActionResult> DeleteSprint(Guid id)
        {
            var sprint = await _sprintsService.GetSprint(id);

            await _sprintsService.DeleteSprint(id);

            return RedirectToAction("GetProject", "Projects", new {id = sprint.Value.ProjectId});
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrManagerPolicy")]
        public async Task<IActionResult> EditSprint(Guid id)
        {
            var sprint = await _sprintsService.GetSprint(id);
            if (sprint.IsSuccess)
            {
                var sprintRequest = new UpdateSprintRequest(
                    id,
                    sprint.Value.ProjectId,
                    sprint.Value.Title,
                    sprint.Value.Description,
                    sprint.Value.DateEnd,
                    sprint.Value.Comment);

                return View(sprintRequest);
            }

            return RedirectToAction("GetProject", "Projects",  new { id = sprint.Value.ProjectId});
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrManagerPolicy")]
        public async Task<IActionResult> EditSprint(UpdateSprintRequest sprintRequest)
        {
            var sprint = SprintModel.Create(
                sprintRequest.ProjectId,
                sprintRequest.Id,
                sprintRequest.Title,
                sprintRequest.Description,
                sprintRequest.DateEnd,
                sprintRequest.Comment,
                Guid.Empty);

            if (sprint.IsFailure)
            {
                return View(sprintRequest);
            }

            var sprintUpdate = await _sprintsService.UpdateSprint(sprint.Value);

            if (sprintUpdate.IsSuccess)
            {
                return RedirectToAction("GetSprint", new { id = sprintRequest.Id });
            }

            return View(sprintRequest);
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrManagerPolicy")]
        public IActionResult SetUser(Guid id)
        {
            ViewData["SprintId"] = id;
            return View("SetUser");
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrManagerPolicy")]
        public async Task<IActionResult> SetUser(Guid id, string email)
        {

            var user = await _usersService.GetUserByEmail(email);

            if (user.IsFailure)
            {
                ViewData["Message"] = $"{user.Error}";
                ViewData["SprintId"] = id;
                return View("SetUser");
            }

            await _sprintsService.SetUser(id, user.Value);

            return RedirectToAction("GetSprint", new { id });
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrManagerPolicy")]
        public async Task<IActionResult> RemoveUser(Guid id)
        {

            await _sprintsService.RemoveUser(id);

            return RedirectToAction("GetSprint", new { id });
        }
    }
}
