using TaskBoard.DAL.Repositories;
using TaskBoard.Domain.Interfaces.Repositories;
using TaskBoard.Domain.Interfaces.Services;
using TaskBoard.Infrastructure.Authentication;
using TaskBoard.Service.Interfice;
using TaskBoard.Service.Services;

namespace TaskBoard.API.Extension
{
    public static class ExtensionService
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProjectsRepository, ProjectsRepository>();
            services.AddScoped<ISprintsRepository, SprintsRepository>();
            services.AddScoped<ITasksRepositoty, TasksRepository>();
            services.AddScoped<IFilesRepositoty, FilesRepository>();
            services.AddScoped<IUsersRepositoty, UsersRepository>();
            services.AddScoped<IBansRepositoty, BanRepository>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IProjectsService, ProjectsService>();
            services.AddScoped<ISprintsService, SprintsService>();
            services.AddScoped<ITasksService, TasksService>();
            services.AddScoped<IFilesService, FilesService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IAccountsService, AccountsService>();
            services.AddScoped<IJwtProvider, JwtProvider>();
        }
    }
}
