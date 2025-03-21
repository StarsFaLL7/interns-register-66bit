using Application.Interfaces;
using Application.Services;
using Domain.DataQuery;
using Domain.Entities;
using InternRegister.Controllers.Base;
using InternRegister.Controllers.Base.Responses;
using InternRegister.Controllers.Interns.Requests;
using InternRegister.Controllers.Interns.Responses;
using InternRegister.Controllers.ProbationProjects.Requests;
using InternRegister.Controllers.ProbationProjects.Responses;
using InternRegister.Controllers.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternRegister.Controllers.ProbationProjects;

[Route("api/projects")]
public class ProbationProjectsController : Controller
{
    private readonly BaseService<ProbationProject> _projectItemsService;
    private readonly IProjectService _projectService;

    public ProbationProjectsController(BaseService<ProbationProject> projectItemsService,
        IProjectService projectService)
    {
        _projectItemsService = projectItemsService;
        _projectService = projectService;
    }
    
    /// <summary>
    /// Получить отфильтрованный список проектов
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(InternListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProjectsFiltered([FromQuery] int? skip, [FromQuery] int? take,
        [FromQuery] string? orderBy, [FromQuery] bool? ascending, [FromQuery] string? search)
    {
        var dataQueryParams = DtoConverter.ConvertBasicDataQuery<ProbationProject>(skip, take, orderBy, ascending ?? true);
        dataQueryParams.Filters = [];
        if (!string.IsNullOrWhiteSpace(search))
        {
            dataQueryParams.Filters.Add(i => i.Title.ToLower().Contains(search.ToLower()));
        }
        dataQueryParams.IncludeParams = new IncludeParams<ProbationProject>
        {
            IncludeProperties = [p => p.Interns]
        };
        if (orderBy == "InternsCount")
        {
            dataQueryParams.Sorting = new SortingParams<ProbationProject>
            {
                OrderBy = c => c.Interns!.Count,
                Ascending = ascending ?? false
            };
        }
        else if (string.IsNullOrWhiteSpace(orderBy))
        {
            dataQueryParams.Sorting = new SortingParams<ProbationProject>
            {
                OrderBy = c => c.Title,
                Ascending = ascending ?? false
            };
        }
        var found = await _projectItemsService.GetAsync(dataQueryParams);
        var foundTotal = await _projectItemsService.GetCountAsync(dataQueryParams);
        
        return Ok(new ProjectsListResponse
        {
            Projects = found.Select(DtoConverter.MapProjectToResponse)
                .ToArray(),
            WithoutPagingCount = foundTotal
        });
    }
    
    /// <summary>
    /// Получить проект по его уникальному идентификатору
    /// </summary>
    [HttpGet("{projectId:guid}")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProjectById([FromRoute] Guid projectId)
    {
        var found = await GetFullProjectInfoAsync(projectId);
        if (found == null)
        {
            return SharedResponses.NotFoundObjectResponse<ProjectResponse>(projectId);
        }
        return Ok(DtoConverter.MapProjectToResponse(found));
    }
    
    /// <summary>
    /// Создать новый проект
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(InternResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateNewProject([FromBody] CreateUpdateProjectRequest dto)
    {
        var dt = DateTimeOffset.FromUnixTimeSeconds(dto.StartDateTimestamp).Date;
        var project = new ProbationProject
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            StartDate = new DateOnly(dt.Year, dt.Month, dt.Day),
            Description = dto.Description,
            Interns = []
        };
        await _projectItemsService.SaveAsync(project);
        await _projectService.ReapplyInternsToProjectAsync(project, dto.InternIds);
        
        return Ok(DtoConverter.MapProjectToResponse(project));
    }
    
    /// <summary>
    /// Обновить информацию о существующем проекте
    /// </summary>
    [HttpPut("{projectId:guid}")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProject([FromBody] CreateUpdateProjectRequest dto, [FromRoute] Guid projectId)
    {
        var dt = DateTimeOffset.FromUnixTimeSeconds(dto.StartDateTimestamp).Date;
        var foundProject = await GetFullProjectInfoAsync(projectId);
        if (foundProject == null)
        {
            return SharedResponses.NotFoundObjectResponse<ProbationProject>(projectId);
        }
        foundProject.Title = dto.Title;
        foundProject.StartDate = new DateOnly(dt.Year, dt.Month, dt.Day);
        foundProject.Description = dto.Description;
        var interns = foundProject.Interns;
        foundProject.Interns = [];
        await _projectItemsService.SaveAsync(foundProject);
        foundProject.Interns = interns;
        await _projectService.ReapplyInternsToProjectAsync(foundProject, dto.InternIds);
        
        return Ok(DtoConverter.MapProjectToResponse(foundProject));
    }
    
    /// <summary>
    /// Удалить проект, если у него нет связанных стажеров
    /// </summary>
    [HttpDelete("{projectId:guid}")]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProject([FromRoute] Guid projectId)
    {
        var foundProject = await GetFullProjectInfoAsync(projectId);
        if (foundProject == null)
        {
            return SharedResponses.NotFoundObjectResponse<ProbationProject>(projectId);
        }

        if (foundProject.Interns!.Count > 0)
        {
            return BadRequest(new BaseStatusResponse
            {
                Completed = false,
                Message = $"Project \"{foundProject.Title}\" has interns connected, so can't be deleted."
            });
        }
        
        await _projectItemsService.TryRemoveAsync(foundProject.Id);
        
        return Ok(new BaseStatusResponse
        {
            Completed = true,
            Message = $"Project \"{foundProject.Title}\" was successfully deleted."
        });
    }

    private async Task<ProbationProject?> GetFullProjectInfoAsync(Guid projectId)
    {
        return await _projectItemsService.GetByIdOrDefaultAsync(projectId, new IncludeParams<ProbationProject>
        {
            IncludeProperties = [i => i.Interns]
        });
    }
}