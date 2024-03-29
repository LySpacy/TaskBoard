using AutoMapper;
using TaskBoard.DAL.Entity;
using TaskBoard.DAL.Model;
using TaskBoard.Domain.Models;

namespace TaskBoard.DAL.Mappings
{
    public class DataBaseMappings : Profile
    {
        public DataBaseMappings()
        {
            CreateMap<ProjectEntity, ProjectModel>();
            CreateMap<SprintEntity, SprintModel>();
            CreateMap<TaskEntity, TaskModel>();
            CreateMap<UserEntity, UserModel>();
            CreateMap<FileEntity, FileModel>();
            CreateMap<BanEntity, BanModel>();
        }
    }
}
