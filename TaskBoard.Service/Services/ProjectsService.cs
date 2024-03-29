using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using TaskBoard.DAL.Repositories;
using TaskBoard.Domain.Interfaces.Repositories;
using TaskBoard.Domain.Interfaces.Services;
using TaskBoard.Domain.Models;

namespace TaskBoard.Service.Services
{
    public class ProjectsService : IProjectsService
    {
        private readonly IProjectsRepository _projectsRepository;

        public ProjectsService(IProjectsRepository projectsRepository)
        {
            _projectsRepository = projectsRepository;
        }

        public async Task<Result<bool>> CreateProject(ProjectModel project)
        {
            try
            {
                await _projectsRepository.Add(project);

                return Result.Success(true);
            }
            catch (Exception ex) 
            {
                return Result.Failure<bool>($"[CreateProject]:{ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<ProjectModel>>> GetProjects()
        {
            try
            {
                var projects = await _projectsRepository.Get();


                return Result.Success<IEnumerable<ProjectModel>>(projects);
            }
            catch (Exception ex)
            {
                return Result.Failure<IEnumerable<ProjectModel>>($"[GetProjects]:{ex.Message}");
            }
        }

        public async Task<Result<ProjectModel>> GetProject(Guid id)
        {
            try
            {
                var project = await _projectsRepository.GetById(id);


                return Result.Success<ProjectModel>(project);
            }
            catch (Exception ex)
            {
                return Result.Failure<ProjectModel> ($"[GetProject]:{ex.Message}");
            }
        }

        public async Task<Result<ProjectModel>> GetProjectWithUserSprints(Guid id, Guid userId)
        {
            try
            {
                var project = await _projectsRepository.GetByIdWithUserSprints(id, userId);


                return Result.Success<ProjectModel>(project);
            }
            catch (Exception ex)
            {
                return Result.Failure<ProjectModel>($"[GetProject]:{ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateProject(ProjectModel project)
        {
            try
            {
                await _projectsRepository.Update(
                    project.Id,
                    project.Title,
                    project.Description);

                return Result.Success<bool>(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>($"[UpdateProject]:{ex.Message}");
            }
        }
        public async Task<Result<bool>> DeleteProject(Guid id)
        {
            try
            {
                await _projectsRepository.Delete(id);

                return Result.Success<bool>(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>($"[DeleteProject]:{ex.Message}");
            }
        }
    }
}
