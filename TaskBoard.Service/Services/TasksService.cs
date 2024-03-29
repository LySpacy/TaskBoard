using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TaskBoard.Domain.Interfaces.Repositories;
using TaskBoard.Domain.Interfaces.Services;
using TaskBoard.Domain.Models;

namespace TaskBoard.Service.Services
{
    public class TasksService : ITasksService
    {
        private readonly ITasksRepositoty _tasksRepositoty;

        public TasksService(ITasksRepositoty tasksRepositoty)
        {
            _tasksRepositoty = tasksRepositoty;
        }

        public async Task<Result<bool>> CreateTask(TaskModel taskModel)
        {
            try
            {
                await _tasksRepositoty.Add(taskModel);

                return Result.Success<bool>(true);
            }
            catch (Exception ex) 
            {
                return Result.Failure<bool>($"[CreateTask]: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteTask(Guid id)
        {
            try
            {
                await _tasksRepositoty.Delete(id);

                return Result.Success<bool>(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>($"[DeleteTask]: {ex.Message}");
            }
        }

        public async Task<Result<TaskModel>> GetTask(Guid id)
        {
            try
            {
                var task = await _tasksRepositoty.GetById(id);

                return Result.Success<TaskModel>(task);
            }
            catch (Exception ex)
            {
                return Result.Failure<TaskModel>($"[GetTask]: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateTask(TaskModel taskModel)
        {
            try
            {
                await _tasksRepositoty.Update(taskModel);

                return Result.Success<bool>(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>($"[DeleteTask]: {ex.Message}");
            }
        }
    }
}
