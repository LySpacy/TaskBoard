using CSharpFunctionalExtensions;
using TaskBoard.Domain.Interfaces.Repositories;
using TaskBoard.Domain.Interfaces.Services;
using TaskBoard.Domain.Models;

namespace TaskBoard.Service.Services
{
    public class SprintsService : ISprintsService
    {
        private readonly ISprintsRepository _sprintsRepository;
        public SprintsService(ISprintsRepository sprintsRepository)
        {
            _sprintsRepository = sprintsRepository;
        }
        public async Task<Result<bool>> CreateSprint(SprintModel sprintModel)
        {
            try
            {
                await _sprintsRepository.Add(sprintModel);

                return Result.Success<bool>(true); 
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>($"[CreateSprint]: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteSprint(Guid id)
        {
            try
            {
                await _sprintsRepository.Delete(id);

                return Result.Success<bool>(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>($"[DeleteSprint]: {ex.Message}");
            }
        }

        public async Task<Result<SprintModel>> GetSprint(Guid id)
        {
            try
            {
                var sprint = await _sprintsRepository.GetById(id);

                return Result.Success<SprintModel>(sprint);
            }
            catch (Exception ex) 
            {
                return Result.Failure<SprintModel>($"[GetSprint]: {ex.Message}");
            }


        }

        public async Task<Result<bool>> UpdateSprint(SprintModel sprintModel)
        {
            try
            {
                await _sprintsRepository.Update(
                    sprintModel.Id, 
                    sprintModel.Title, 
                    sprintModel.Description, 
                    sprintModel.Comment, 
                    sprintModel.DateEnd);

                return Result.Success<bool>(true);
            }
            catch (Exception ex) 
            {
                return Result.Failure<bool>($"[UpdateSprint]: {ex.Message}");
            }
        }

        public async Task<Result<bool>> SetUser(Guid id, UserModel user)
        {
            try
            {
                await _sprintsRepository.SetUser(id, user.Id);

                return Result.Success<bool>(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>($"[UpdateSprint]: {ex.Message}");
            }
        }

        public async Task<Result<bool>> RemoveUser(Guid id)
        {
            try
            {
                await _sprintsRepository.RemoveUser(id);

                return Result.Success<bool>(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>($"[RemoveUser]: {ex.Message}");
            }
        }
    }
}
