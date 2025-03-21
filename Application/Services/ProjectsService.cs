using Application.Interfaces;
using Domain.DataQuery;
using Domain.Entities;

namespace Application.Services;

public class ProjectsService : IProjectService
{
    private readonly BaseService<Intern> _internService;

    public ProjectsService(BaseService<Intern> internService)
    {
        _internService = internService;
    }

    public async Task ReapplyInternsToProjectAsync(ProbationProject project, Guid[] internIds)
    {
        project.Interns ??= [];
        var newInterns = await _internService.GetAsync(new DataQueryParams<Intern>
        {
            Expression = i => internIds.Contains(i.Id)
        });
        foreach (var intern in project.Interns.UnionBy(newInterns, i => i.Id))
        {
            intern.ProbationProject = null;
            intern.ProbationProjectId = null;
            if (internIds.Contains(intern.Id))
            {
                intern.ProbationProjectId = project.Id;
            }

            await _internService.SaveAsync(intern);
        }
    }
}