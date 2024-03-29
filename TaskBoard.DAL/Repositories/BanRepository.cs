using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using TaskBoard.DAL.Entity;
using TaskBoard.Domain.Enum;
using TaskBoard.Domain.Interfaces.Repositories;
using TaskBoard.Domain.Models;

namespace TaskBoard.DAL.Repositories
{
    public  class BanRepository : IBansRepositoty
    {
        private readonly TaskBoardDbContext _dbContext;
        private readonly IMapper _mapper;
        public BanRepository(TaskBoardDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task Baned(Guid id, string cause)
        {
            await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(x => x
                .SetProperty(u => u.Role, UserRole.Ban));

            var linkedSprints = _dbContext.Sprints.Where(s => s.UserId == id);
            foreach (var sprint in linkedSprints)
            {
                sprint.UserId = null;
            }

            var ban = new BanEntity()
            {
                UserId = id,
                Сause = cause
            };

            await _dbContext.AddAsync(ban);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UnBaned(Guid id)
        {
            await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(x => x
                .SetProperty(u => u.Role, UserRole.User));


            await _dbContext.BlackList
                .Where(b => b.UserId == id)
                .ExecuteDeleteAsync();

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<BanModel>> GetBanList()
        {
            var banEntity = await _dbContext.BlackList
                        .AsNoTracking()
                        .Include(b => b.User)
                        .ToListAsync();

            return _mapper.Map<List<BanModel>>(banEntity);
        }

        public async Task<BanModel> GetBanUserById(Guid id)
        {
            var banEntity = await _dbContext.BlackList
               .AsNoTracking()
               .Include(b => b.User)
               .FirstOrDefaultAsync(b => b.UserId == id);

            return _mapper.Map<BanModel>(banEntity);
        }
    }
}
