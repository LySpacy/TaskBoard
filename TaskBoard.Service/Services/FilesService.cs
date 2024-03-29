using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TaskBoard.Domain.Interfaces.Repositories;
using TaskBoard.Domain.Interfaces.Services;
using TaskBoard.Domain.Models;

namespace TaskBoard.Service.Services
{
    public class FilesService : IFilesService
    {
        private readonly IFilesRepositoty _filesRepositoty;
        public FilesService(IFilesRepositoty filesRepositoty)
        {
            _filesRepositoty = filesRepositoty;
        }
        public async Task<Result<bool>> AddFileToSprint(Guid ownerId, FileModel file)
        {
            try
            {
                file.OwnerId = ownerId;
                await _filesRepositoty.AddToSprint(file);

                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>($"{ex.Message}");
            }
        }
        public async Task<Result<bool>> AddFileToTask(Guid ownerId, FileModel file)
        {
            try
            {
                file.OwnerId = ownerId;
                await _filesRepositoty.AddToTask(file);

                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>($"{ex.Message}");
            }
        }

        public async Task<Result<FileModel>> CreateFile(IFormFile titleFile, string path)
        {
            try
            {
                var fileName = Path.GetFileName(titleFile.FileName);
                var filePath = Path.Combine(path, fileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await titleFile.CopyToAsync(stream);
                }

                var file = FileModel.Create(filePath);
                
                return Result.Success<FileModel>(file.Value);
            }
            catch(Exception ex)
            {
                return Result.Failure<FileModel>(ex.Message);
            }
        }

        public async Task<Result<bool>> DeleteFile(Guid id)
        {
            try
            {
                await _filesRepositoty.Delete(id);

                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>($"{ex.Message}");
            }
        }

        public async Task<Result<FileModel>> GetFile(Guid id)
        {
            try
            {
                var file = await _filesRepositoty.GetById(id);
                if (file == null)
                {
                    return Result.Failure<FileModel>($"Файл не найден");
                }
                return Result.Success<FileModel>(file);
            }
            catch (Exception ex)
            {
                return Result.Failure<FileModel>($"{ex.Message}");
            }
        }
    }
}
