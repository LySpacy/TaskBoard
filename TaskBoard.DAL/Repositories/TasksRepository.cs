using Microsoft.EntityFrameworkCore;
using TaskBoard.DAL.Model;
using AutoMapper;
using TaskBoard.Domain.Models;
using TaskBoard.Domain.Interfaces.Repositories;
using TaskBoard.Domain.Extensions;

namespace TaskBoard.DAL.Repositories
{
    public class TasksRepository : ITasksRepositoty
    {
        private readonly TaskBoardDbContext _dbContext;
        private readonly IMapper _mapper;
        public TasksRepository(TaskBoardDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task Add(TaskModel taskModel)
        {
            var task = new TaskEntity()
            {
                SprintId = taskModel.SprintId,
                Title = taskModel.Title,
                Description = taskModel.Description,
                Comment = taskModel.Comment,
                Status = taskModel.Status
            };
            
            await _dbContext.AddAsync(task);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<TaskModel>> GetBySprint(Guid sprintId)
        {
            var taskEntity = await _dbContext.Tasks
                 .AsNoTracking()
                 .Where(s => s.SprintId == sprintId)
                 .OrderBy(s => s.Title)
                 .ToListAsync();

            return _mapper.Map<List<TaskModel>>(taskEntity);
        }

        public async Task<TaskModel> GetById(Guid id)
        {
            var task = await _dbContext.Tasks
                 .AsNoTracking()
                 .Include(t => t.Files)
                 .Include(t => t.Sprint)
                     .ThenInclude(s => s.User)
                 .FirstOrDefaultAsync(t => t.Id == id);

            return _mapper.Map<TaskModel>(task);
        }

        public async Task Delete(Guid id) 
        {
            await _dbContext.Tasks
                .Where(t => t.Id == id)
                .ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(TaskModel taskModel)
        {
            await _dbContext.Tasks
                .Where(t => t.Id == taskModel.Id)
                .ExecuteUpdateAsync(x => x
                .SetProperty(t => t.Title, taskModel.Title)
                .SetProperty(t => t.Description, taskModel.Description)
                .SetProperty(t => t.Comment, taskModel.Comment)
                .SetProperty(t => t.Status, taskModel.Status));

            await _dbContext.SaveChangesAsync();
        }
    }
}
