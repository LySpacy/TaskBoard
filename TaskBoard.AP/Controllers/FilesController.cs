using Microsoft.AspNetCore.Mvc;
using TaskBoard.Domain.Interfaces.Services;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace TaskBoard.API.Controllers
{
    public class FilesController : Controller
    {
        private readonly string _staticFilesPath =
           Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "StaticFiles");

        private readonly IFilesService _filesService;
        private readonly ISprintsService _sprintService;
        private readonly ITasksService _tasksService;
        public FilesController(IFilesService filesService, ISprintsService sprintsService, ITasksService ITasksService)
        {
            _filesService = filesService;
            _sprintService = sprintsService;
            _tasksService = ITasksService;
        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Менеджер")]
        public async Task<IActionResult> UploadFileToSprint(Guid sprintId, IFormFile file)
        {
            var path = Path.Combine(_staticFilesPath, "SprintFiles");
            var fileModel = await _filesService.CreateFile(file, path);

            if (fileModel.IsFailure)
            {
                return BadRequest(fileModel.Error);
            }

            await _filesService.AddFileToSprint(sprintId, fileModel.Value);

            return RedirectToAction("GetSprint", "Sprints", new { id = sprintId }); ;

        }

        [HttpPost]
        [Authorize(Policy = "AllNotBanedUsers")]
        public async Task<IActionResult> UploadFileToTaks(Guid taskId, IFormFile file)
        {
            var path = Path.Combine(_staticFilesPath, "TaskFiles");
            var fileModel = await _filesService.CreateFile(file, path);

            if (fileModel.IsFailure)
            {
                return BadRequest(fileModel.Error);
            }

            await _filesService.AddFileToTask(taskId, fileModel.Value);

            return RedirectToAction("GetTask", "Tasks", new { id = taskId }); ;

        }

        [HttpPost]
        [Authorize(Roles = "Администратор, Менеджер")]
        public async Task<IActionResult> DeleteFile(Guid id)
        {
                  
            var file = await _filesService.GetFile(id);

            var deletionResult  = await _filesService.DeleteFile(id);
            if (deletionResult.IsFailure)
            {
                return StatusCode(500, "Не удалось удалить файл.");
            }

            var sprint = await _sprintService.GetSprint(file.Value.OwnerId);
            if (sprint.IsSuccess)
            {
                var path = Path.Combine(_staticFilesPath, "SprintFiles", file.Value.FileName);
                System.IO.File.Delete(path);
                return RedirectToAction("GetSprint", "Sprints", new { id = file.Value.OwnerId });
            }

            var task = await _tasksService.GetTask(file.Value.OwnerId);
            if (task.IsSuccess)
            {
                var path = Path.Combine(_staticFilesPath, "TaskFiles", file.Value.FileName);
                System.IO.File.Delete(path);
                return RedirectToAction("GetTask", "Tasks", new { id = file.Value.OwnerId });
            }

            return RedirectToAction("GetSprint", "Sprints", new { id = file.Value.OwnerId }); 

        }

        [Authorize(Policy = "AllNotBanedUsers")]
        public async Task<IActionResult> DownloadFile(Guid id)
        {
            var file = await _filesService.GetFile(id);

            if (file.IsFailure)
            {
                return NotFound();
            }
            var path = Path.Combine(_staticFilesPath, "SprintFiles");

            var filePath = Path.Combine(path, file.Value.FileName);

            if (!System.IO.File.Exists(filePath))
            {
                path = Path.Combine(_staticFilesPath, "TaskFiles");

                filePath = Path.Combine(path, file.Value.FileName);
            }

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var fileName = Path.GetFileName(file.Value.FileName);
            var contentType = "application/octet-stream"; 
            return File(fileStream, contentType, fileName);
        }
    }
}
