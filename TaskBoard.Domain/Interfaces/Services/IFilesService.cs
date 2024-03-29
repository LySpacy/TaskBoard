using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using TaskBoard.Domain.Models;

namespace TaskBoard.Domain.Interfaces.Services
{
    public interface IFilesService
    {
        Task<Result<FileModel>> CreateFile(IFormFile titleFile, string path);
        Task<Result<bool>> AddFileToSprint(Guid ownerId, FileModel file);
        Task<Result<bool>> AddFileToTask(Guid ownerId, FileModel file);
        Task<Result<bool>> DeleteFile(Guid id);
        Task<Result<FileModel>> GetFile(Guid id);
    }
}
