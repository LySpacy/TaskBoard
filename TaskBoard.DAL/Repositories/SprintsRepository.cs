using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using TaskBoard.DAL.Model;
using TaskBoard.Domain.Interfaces.Repositories;
using TaskBoard.Domain.Models;

namespace TaskBoard.DAL.Repositories
{
    public class SprintsRepository : ISprintsRepository
    {
        private readonly TaskBoardDbContext _dbContext;
        private readonly IMapper _mapper;

        public SprintsRepository(TaskBoardDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task Add(SprintModel sprintModel)
        {
            var sprint = new SprintEntity()
            {
                Title = sprintModel.Title,
                Description = sprintModel.Description,
                Comment = sprintModel.Comment,
                DateEnd = sprintModel.DateEnd,
                ProjectId = sprintModel.ProjectId
            };

            //await _dbContext.AddAsync(sprint);
            _dbContext.Entry(sprint).State = EntityState.Added;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<SprintModel>> GetByProject(Guid projectId)
        {
            var sprintEntity = await _dbContext.Sprints
               .AsNoTracking()
               .Where(p => p.ProjectId == projectId)
               .OrderBy(p => p.DateStart)
               .ToListAsync();

            return _mapper.Map<List<SprintModel>>(sprintEntity);
        }

        public async Task<SprintModel> GetById(Guid id)
        {
            var sprintEntity = await _dbContext.Sprints
                .AsNoTracking()
                .Include(s => s.User)
                .Include(s => s.Tasks)
                .Include(s => s.Files)
                .FirstOrDefaultAsync(s => s.Id == id);

            return _mapper.Map<SprintModel>(sprintEntity);
        }

        public async Task Update(Guid id, string title, string description, string comment, DateTime dateEnd)
        {
            await _dbContext.Sprints
                .Where(s => s.Id == id)
                .Include(p => p.Project)
                .ExecuteUpdateAsync(x => x
                .SetProperty(s => s.Title, title)
                .SetProperty(s => s.Description, description)
                .SetProperty(s => s.Comment, comment)
                .SetProperty(s => s.DateEnd, dateEnd));

            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
           await _dbContext.Sprints
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();
        }

        public async Task SetUser(Guid id, Guid userId)
        {
            var sprintEntity = await _dbContext.Sprints
                .Where(s => s.Id == id)
                .ExecuteUpdateAsync(x => x
                .SetProperty(s => s.UserId, userId));

            var user = await _dbContext.Users
                .Include(u => u.Sprints)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var sprint = await _dbContext.Sprints
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            user.Sprints.Add(sprint);

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveUser(Guid id)
        {
            var sprintEntity = await _dbContext.Sprints
                .Where(s => s.Id == id)
                .ExecuteUpdateAsync(x => x
                .SetProperty(s => s.UserId, _ => null));

            await _dbContext.SaveChangesAsync();
        }
    }
}
