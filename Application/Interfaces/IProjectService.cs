using Domain.Entities;

namespace Application.Interfaces;

public interface IProjectService
{
    public Task ReapplyInternsToProjectAsync(ProbationProject project, Guid[] internIds);
}