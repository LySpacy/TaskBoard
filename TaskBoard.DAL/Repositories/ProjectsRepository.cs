using Microsoft.EntityFrameworkCore;
using TaskBoard.Domain.Models;
using TaskBoard.DAL.Model;
using AutoMapper;
using TaskBoard.Domain.Interfaces.Repositories;

namespace TaskBoard.DAL.Repositories
{
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly TaskBoardDbContext _dbContext;
        private readonly IMapper _mapper;
        public ProjectsRepository(TaskBoardDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<List<ProjectModel>> Get()
        {
            var projectEntity = await _dbContext.Projects
                .AsNoTracking()
                .Include(p => p.Sprints)
                .ToListAsync();

            return _mapper.Map<List<ProjectModel>>(projectEntity);
        }
        public async Task<ProjectModel> GetById(Guid id)
        {
            var projectEntity = await _dbContext.Projects
                .AsNoTracking()
                .Include(p => p.Sprints)
                    .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            return _mapper.Map<ProjectModel>(projectEntity);
        }

        public async Task<List<ProjectModel>> GetByPage(int page, int pageSize)
        {
            var projectEntity = await _dbContext.Projects
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<List<ProjectModel>>(projectEntity);
        }

        public async Task<ProjectModel> GetByIdWithUserSprints(Guid id, Guid userId)
        {
            var projectEntity = await _dbContext.Projects
                .AsNoTracking()
                .Include(p => p.Sprints.Where(s => s.UserId == userId))
                    .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            return _mapper.Map<ProjectModel>(projectEntity);
        }

        public async Task Add(ProjectModel projectModel)
        {
            var projectEntity = new ProjectEntity()
            {
                Id = projectModel.Id,
                Title = projectModel.Title,
                Description = projectModel.Description
            };

            await _dbContext.Projects.AddAsync(projectEntity);
            await _dbContext.SaveChangesAsync();
        }


        public async Task Update(Guid id, string title, string description)
        {
            await _dbContext.Projects
                .Where(p => p.Id == id)
                .ExecuteUpdateAsync(x => x
                .SetProperty(p => p.Title, title)
                .SetProperty(p => p.Description, description));
                

            
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            await _dbContext.Projects
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync();           

            await _dbContext.SaveChangesAsync();
        }
    }
}
