using TaskBoard.Domain.Models;

namespace TaskBoard.Domain.Interfaces.Repositories
{
    public interface IFilesRepositoty
    {
        Task<FileModel> GetById(Guid id);
        Task AddToSprint(FileModel fileModel);
        Task AddToTask(FileModel fileModel);
        Task Delete(Guid id);

    }

}
