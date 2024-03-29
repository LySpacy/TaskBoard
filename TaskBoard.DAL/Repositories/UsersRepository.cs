using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskBoard.DAL.Entity;
using TaskBoard.DAL.Model;
using TaskBoard.Domain.Enum;
using TaskBoard.Domain.Helpers;
using TaskBoard.Domain.Interfaces.Repositories;
using TaskBoard.Domain.Models;

namespace TaskBoard.DAL.Repositories
{
    public class UsersRepository : IUsersRepositoty
    {
        private readonly TaskBoardDbContext _dbContext;
        private readonly IMapper _mapper;
        public UsersRepository(TaskBoardDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

       public async Task UpdateRole(Guid id, UserRole role)
        {
            await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(x => x
                .SetProperty(u => u.Role, role));

            await _dbContext.SaveChangesAsync();
        }
        public async Task Add(UserModel userModel)
        {
            var user = new UserEntity()
            {
                Name = userModel.Name,
                Email = userModel.Email,
                PasswordHash = userModel.PasswordHash,
                Role = userModel.Role
            };
            await _dbContext.AddAsync(user);

            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<UserModel>> Get()
        {
            var userEntity = await _dbContext.Users
                        .AsNoTracking()
                        .ToListAsync();

            return _mapper.Map<List<UserModel>>(userEntity);
        }

        public async Task<UserModel> GetById(Guid id)
        {
            var userEntity = await _dbContext.Users
               .AsNoTracking()
               .Include(u => u.Sprints)
               .Include(u => u.Ban)
               .FirstOrDefaultAsync(u => u.Id == id);

            return _mapper.Map<UserModel>(userEntity);
        }

        public async Task<UserModel> GetByEmail(string email)
        {
            var userEntity = await _dbContext.Users
               .AsNoTracking()
               .Include(u => u.Sprints)
               .Include(u => u.Ban)
               .FirstOrDefaultAsync(u => u.Email == email);

            return _mapper.Map<UserModel>(userEntity);
        }
        public async Task Delete(Guid id)
        {
            await _dbContext.Users
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync();

            var linkedSprints = _dbContext.Sprints.Where(s => s.UserId == id);
            foreach (var sprint in linkedSprints)
            {
                sprint.UserId = null;
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Guid id, string login, string passwordHash, string email, UserRole role)
        {
            await _dbContext.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(x => x
                .SetProperty(u => u.Name, login)
                .SetProperty(u => u.PasswordHash, passwordHash)
                .SetProperty(u => u.Email, email)
                .SetProperty(u => u.Role, role));

            await _dbContext.SaveChangesAsync();
        }

    }
}
