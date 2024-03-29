using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskBoard.DAL.Model;
using TaskBoard.Domain.Interfaces.Repositories;
using TaskBoard.Domain.Models;

namespace TaskBoard.DAL.Repositories
{
    public class FilesRepository : IFilesRepositoty
    {
        private readonly TaskBoardDbContext _dbContext;
        private readonly IMapper _mapper;
        public FilesRepository(TaskBoardDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task AddToSprint(FileModel fileModel)
        {
            var file = new FileEntity()
            {
                SprintId = fileModel.OwnerId,
                FileName = fileModel.FileName
            };

            await _dbContext.AddAsync(file);

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddToTask(FileModel fileModel)
        {
            var file = new FileEntity()
            {
                TaskId = fileModel.OwnerId,
                FileName = fileModel.FileName
            };

            await _dbContext.AddAsync(file);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<FileModel> GetById(Guid id)
        {
            var fileEntity = await _dbContext.Files
               .AsNoTracking()
               .FirstOrDefaultAsync(p => p.Id == id);

            var fileModel = FileModel.Create(fileEntity.FileName).Value;
            fileModel.Id = id;
            fileModel.OwnerId = fileModel.OwnerId = fileEntity.TaskId ?? fileEntity.SprintId ?? Guid.Empty;

            return fileModel;
        }
        public async Task Delete(Guid id)
        {
            await _dbContext.Files
                .Where(f => f.Id == id)
                .ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();
        }
    }
}
