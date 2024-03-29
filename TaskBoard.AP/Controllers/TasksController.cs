using Microsoft.AspNetCore.Mvc;
using TaskBoard.Domain.Interfaces.Services;
using TaskBoard.Domain.Models;
using TaskBoard.Domain.Enum;
using TaskBoard.Domain.Extensions;
using TaskBoard.API.Contracts.Task;
using Microsoft.AspNetCore.Authorization;

namespace TaskBoard.API.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITasksService _tasksService;
        public TasksController(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrManagerPolicy")]
        public IActionResult CreateTask(Guid idSprint)
        {
            var taskRequest = new CreateTaskRequest(
                idSprint, 
                string.Empty, 
                string.Empty, 
                StatusTask.Expectation, 
                string.Empty);

            return View(taskRequest);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrManagerPolicy")]
        public async Task<IActionResult> CreateSprint(CreateTaskRequest taskRequest)
        {
            var task = TaskModel.Create(
                taskRequest.SprintId,
                Guid.Empty,
                taskRequest.Title,
                taskRequest.Description,
                taskRequest.Comment);

            if (task.IsFailure)
            {
                return View(taskRequest);
            }

            await _tasksService.CreateTask(task.Value);

            return RedirectToAction("GetSprint", "Sprints", new { id = taskRequest.SprintId });
        }

        [HttpGet]
        [Authorize(Policy = "AllNotBanedUsers")]
        public async Task<IActionResult> GetTask(Guid id)
        {
            var task = await _tasksService.GetTask(id);

            if (task.IsSuccess)
            {
                return View(task.Value);
            }

            return View(task.Error);
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrManagerPolicy")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var task = await _tasksService.GetTask(id);

            await _tasksService.DeleteTask(id);

            return RedirectToAction("GetSprint", "Sprints", new { id = task.Value.SprintId });
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrManagerPolicy")]
        public async Task<IActionResult> EditTask(Guid id)
        {
            var task = await _tasksService.GetTask(id);
            
            if (task.IsSuccess)
            {
                var taksRequest = new UpdateTaskRequest(
                    id,
                    task.Value.SprintId,
                    task.Value.Title,
                    task.Value.Description,
                    task.Value.Status,
                    task.Value.Comment);

                return View(taksRequest);
            }

            return BadRequest("Задача не была найдена");
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrManagerPolicy")]
        public async Task<IActionResult> EditTask(UpdateTaskRequest taskRequest)
        {
            var task = TaskModel.Create(
               taskRequest.SprintId,
               taskRequest.Id,
               taskRequest.Title,
               taskRequest.Description,
               taskRequest.Comment);

            if (task.IsFailure)
            {
                return View(taskRequest);
            }
            task.Value.UpdateStatus(taskRequest.Status);

            var taskUpdate = await _tasksService.UpdateTask(task.Value);

            if (taskUpdate.IsSuccess)
            {
                return RedirectToAction("GetTask", new { id = taskRequest.Id });
            }

            return View(taskRequest);
        }
        
        [HttpGet]
        [Authorize(Policy = "AllNotBanedUsers")]
        public async Task<IActionResult> StartTask(Guid id)
        {
            var task = await _tasksService.GetTask(id);

            if (task.IsFailure)
            {
                return BadRequest($"{task.Error}");
            }

            task.Value.UpdateStatus(StatusTask.InProgress);

            await _tasksService.UpdateTask(task.Value);

            return RedirectToAction("GetTask", new { id });
        }

        [HttpGet]
        [Authorize(Policy = "AllNotBanedUsers")]
        public async Task<IActionResult> PostponeTask(Guid id)
        {
            var task = await _tasksService.GetTask(id);

            if (task.IsFailure)
            {
                return BadRequest($"{task.Error}");
            }

            task.Value.UpdateStatus(StatusTask.Expectation);

            await _tasksService.UpdateTask(task.Value);

            return RedirectToAction("GetTask", new { id });
        }

        [HttpGet]
        [Authorize(Policy = "AllNotBanedUsers")]
        public async Task<IActionResult> CompleteTask(Guid id)
        {
            var task = await _tasksService.GetTask(id);

            if (task.IsFailure)
            {
                return BadRequest($"{task.Error}");
            }

            task.Value.UpdateStatus(StatusTask.Сompleted);

            await _tasksService.UpdateTask(task.Value);

            return RedirectToAction("GetTask", new { id });
        }
    }
}
